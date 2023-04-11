﻿using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using TheGuideToTheNewEden.Core.Models.Character;
using System.IO;
using System.Diagnostics;
using TheGuideToTheNewEden.WinUI.Helpers;
using System.Threading.Tasks;

namespace TheGuideToTheNewEden.WinUI.Services
{
    internal static class CharacterService
    {
        private static string ClientId = "8d0da2b105324ead932f60f32b1a55fb";
        private static string RedirectUri = "eveauth-qedsd-neweden2:///";
        private static string StructureMarketScope = "esi-universe.read_structures.v1 esi-markets.structure_markets.v1";
        private static string AllScope = "publicData esi-calendar.respond_calendar_events.v1 esi-calendar.read_calendar_events.v1 esi-location.read_location.v1 esi-location.read_ship_type.v1 esi-mail.organize_mail.v1 esi-mail.read_mail.v1 esi-mail.send_mail.v1 esi-skills.read_skills.v1 esi-skills.read_skillqueue.v1 esi-wallet.read_character_wallet.v1 esi-wallet.read_corporation_wallet.v1 esi-search.search_structures.v1 esi-clones.read_clones.v1 esi-characters.read_contacts.v1 esi-universe.read_structures.v1 esi-bookmarks.read_character_bookmarks.v1 esi-killmails.read_killmails.v1 esi-corporations.read_corporation_membership.v1 esi-assets.read_assets.v1 esi-planets.manage_planets.v1 esi-fleets.read_fleet.v1 esi-fleets.write_fleet.v1 esi-ui.open_window.v1 esi-ui.write_waypoint.v1 esi-characters.write_contacts.v1 esi-fittings.read_fittings.v1 esi-fittings.write_fittings.v1 esi-markets.structure_markets.v1 esi-corporations.read_structures.v1 esi-characters.read_loyalty.v1 esi-characters.read_opportunities.v1 esi-characters.read_chat_channels.v1 esi-characters.read_medals.v1 esi-characters.read_standings.v1 esi-characters.read_agents_research.v1 esi-industry.read_character_jobs.v1 esi-markets.read_character_orders.v1 esi-characters.read_blueprints.v1 esi-characters.read_corporation_roles.v1 esi-location.read_online.v1 esi-contracts.read_character_contracts.v1 esi-clones.read_implants.v1 esi-characters.read_fatigue.v1 esi-killmails.read_corporation_killmails.v1 esi-corporations.track_members.v1 esi-wallet.read_corporation_wallets.v1 esi-characters.read_notifications.v1 esi-corporations.read_divisions.v1 esi-corporations.read_contacts.v1 esi-assets.read_corporation_assets.v1 esi-corporations.read_titles.v1 esi-corporations.read_blueprints.v1 esi-bookmarks.read_corporation_bookmarks.v1 esi-contracts.read_corporation_contracts.v1 esi-corporations.read_standings.v1 esi-corporations.read_starbases.v1 esi-industry.read_corporation_jobs.v1 esi-markets.read_corporation_orders.v1 esi-corporations.read_container_logs.v1 esi-industry.read_character_mining.v1 esi-industry.read_corporation_mining.v1 esi-planets.read_customs_offices.v1 esi-corporations.read_facilities.v1 esi-corporations.read_medals.v1 esi-characters.read_titles.v1 esi-alliances.read_contacts.v1 esi-characters.read_fw_stats.v1 esi-corporations.read_fw_stats.v1 esi-characterstats.read.v1";
        private static readonly string AuthFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "Auth.json");
        public static ObservableCollection<CharacterOauth> CharacterOauths { get; private set; }
        public static void Init()
        {
            CoreConfig.ClientId = ClientId;
            CoreConfig.Scope = AllScope;
            CoreConfig.ESICallback = RedirectUri;
            if (File.Exists(AuthFilePath))
            {
                string json = File.ReadAllText(AuthFilePath);
                CharacterOauths = JsonConvert.DeserializeObject<ObservableCollection<CharacterOauth>>(json);
            }
            else
            {
                CharacterOauths = new ObservableCollection<CharacterOauth>();
            }
        }
        private static void Save()
        {
            if (CharacterOauths != null)
            {
                string json = JsonConvert.SerializeObject(CharacterOauths);
                string folder = Path.GetDirectoryName(AuthFilePath);
                if(!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllText(AuthFilePath, json);
            }
        }

        public static void Add(CharacterOauth characterOauth)
        {
            CharacterOauths.Add(characterOauth);
            Save();
        }

        public static void Remove(CharacterOauth characterOauth)
        {
            if (CharacterOauths.Remove(characterOauth))
            {
                Save();
            }
        }

        public static async Task<CharacterOauth> AddAsync(Core.Enums.GameServerType server = Core.Enums.GameServerType.Tranquility)
        {
            GetAuthorizeByBrower();
            var result = await AuthHelper.WaitingAuthAsync();
            if (result != null)
            {
                return await HandelCharacterOatuhProtocolAsync(result);
            }
            else
            {
                return null;
            }
        }

        public static void GetAuthorizeByBrower(Core.Enums.GameServerType server = Core.Enums.GameServerType.Tranquility)
        {
            string uri = null;
            if (server == Core.Enums.GameServerType.Tranquility)
            {
                uri = $"https://login.eveonline.com/v2/oauth/authorize?response_type=code&client_id={ClientId}&redirect_uri={RedirectUri}&state=test&scope={AllScope}";
            }
            else
            {

            }
            var sInfo = new ProcessStartInfo(uri)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);

        }

        public static async Task<CharacterOauth> HandelCharacterOatuhProtocolAsync(string uri)
        {
            var array = uri.Split(new char[2] { '=', '&' });
            string code = array[1];
            var token = await Core.Services.CharacterService.GetOauthTokenAsync(ClientId, code);
            if (token != null)
            {
                var vorifyToken = await Core.Services.CharacterService.GetVorifyTokenAsync(ClientId, token.Access_token);
                if (vorifyToken != null)
                {
                    CharacterOauth characterOauth = new CharacterOauth(token, vorifyToken);
                    Add(characterOauth);
                    return characterOauth;
                }
            }
            return null;
        }
        public static void HandelStructureOauthProtocol(string uri)
        {

        }
    }
}
