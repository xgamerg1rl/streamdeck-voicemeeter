﻿using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceMeeter
{
    class VMModify : PluginBase
    {
        private enum ParamTypeEnum
        {
            gain = 0,
            comp = 1,
            gate = 2,
            solo = 3,
            mono = 4,
            pan_x = 5,
            pan_y = 6,
            color_x = 7,
            color_y = 8,
            fx_x = 9,
            fx_y = 10,
            eqgain1 = 11,
            eqgain2 = 12,
            eqgain3 = 13,
            mc      = 14,
            audibility = 15
        }

        private enum TitleTypeEnum
        {
            VMLive = 0,
            None = 1
        }

        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.ParamType = ParamTypeEnum.gain;
                instance.Strip = "Strip";
                instance.StripNum = 0;
                instance.SetValue = String.Empty;
                instance.LongPressValue = String.Empty;
                instance.TitleType = TitleTypeEnum.VMLive;

                return instance;
            }

            [JsonProperty(PropertyName = "paramType")]
            public ParamTypeEnum ParamType { get; set; }

            [JsonProperty(PropertyName = "strip")]
            public string Strip { get; set; }

            [JsonProperty(PropertyName = "stripNum")]
            public int StripNum { get; set; }

            [JsonProperty(PropertyName = "setValue")]
            public string SetValue { get; set; }

            [JsonProperty(PropertyName = "longPressValue")]
            public string LongPressValue { get; set; }

            [JsonProperty(PropertyName = "titleType")]
            public TitleTypeEnum TitleType { get; set; }
        }

        #region Private members

        private const int LONG_KEYPRESS_LENGTH = 1;

        private PluginSettings settings;
        private bool keyPressed = false;
        private DateTime keyPressStart;

        #endregion

        #region Public Methods

        public VMModify(SDConnection connection, JObject settings) : base(connection, settings)
        {
            if (settings == null || settings.Count == 0)
            {
                this.settings = PluginSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = settings.ToObject<PluginSettings>();
            }
        }

        public void LongKeyPressed()
        {
            float value;
            if (!String.IsNullOrEmpty(settings.LongPressValue) && float.TryParse(settings.LongPressValue, out value))
            {
                VMManager.Instance.SetParam(BuildDeviceName(), value);
            }
        }

        #endregion

        #region PluginBase

        public async override void KeyPressed()
        {
            if (!VMManager.Instance.IsConnected)
            {
                await Connection.ShowAlert();
                return;
            }

            // Used for long press
            keyPressed = true;
            keyPressStart = DateTime.Now;

            float value;
            if (!String.IsNullOrEmpty(settings.SetValue) && float.TryParse(settings.SetValue, out value))
            {
                VMManager.Instance.SetParam(BuildDeviceName(), value);
            }
        }

        public override void KeyReleased()
        {
            keyPressed = false;
        }

        public async override void OnTick()
        {
            // Stream Deck calls this function every second, 
            // so this is the best place to determine if we need to call the long keypress
            if (!String.IsNullOrEmpty(settings.LongPressValue) && keyPressed && (DateTime.Now - keyPressStart).TotalSeconds > LONG_KEYPRESS_LENGTH)
            {
                LongKeyPressed();
            }

            if (settings.TitleType == TitleTypeEnum.VMLive)
            {
                await Connection.SetTitleAsync(VMManager.Instance.GetParam(BuildDeviceName()));
            }
        }

        public override void Dispose()
        {
        }

        public async override void UpdateSettings(JObject payload)
        {
            if (payload["property_inspector"] != null)
            {
                switch (payload["property_inspector"].ToString().ToLower())
                {
                    case "propertyinspectorconnected":
                        await Connection.SendToPropertyInspectorAsync(JObject.FromObject(settings));
                        break;

                    case "propertyinspectorwilldisappear":
                        await Connection.SetSettingsAsync(JObject.FromObject(settings));
                        break;

                    case "updatesettings":
                        settings.ParamType = (ParamTypeEnum)Enum.Parse(typeof(ParamTypeEnum), (string)payload["paramType"]);
                        settings.Strip = (string)payload["strip"];
                        settings.StripNum = (int)payload["stripNum"];
                        settings.SetValue = (string)payload["setValue"];
                        settings.LongPressValue = (string)payload["longPressValue"];
                        settings.TitleType = (TitleTypeEnum)Enum.Parse(typeof(TitleTypeEnum), (string)payload["titleType"]);
                        await Connection.SetSettingsAsync(JObject.FromObject(settings));
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        private string BuildDeviceName()
        {
            return $"{settings.Strip}[{settings.StripNum}].{settings.ParamType.ToString()}";
        }

        #endregion
    }
}
