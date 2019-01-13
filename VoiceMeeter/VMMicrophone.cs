﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceMeeter
{
    class VMMicrophone : IPluginable
    {
        private enum MicTypeEnum
        {
            SingleMode = 0,
            Toggle = 1,
            PTT = 2
        }

        private enum ImageTypeEnum
        {
            Microphone = 0,
            Speaker = 1,
            OnOff = 2,
            Microphone2 = 3
        }

        private class InspectorSettings : SettingsBase
        {
            public static InspectorSettings CreateDefaultSettings()
            {
                InspectorSettings instance = new InspectorSettings();
                instance.MicType = MicTypeEnum.Toggle;
                instance.Strip = "Strip";
                instance.StripNum = 0;
                instance.SingleValue = String.Empty;
                instance.ImageType = ImageTypeEnum.Microphone;

                return instance;
            }
            
            [JsonProperty(PropertyName = "micType")]
            public MicTypeEnum MicType { get; set; }

            [JsonProperty(PropertyName = "strip")]
            public string Strip { get; set; }

            [JsonProperty(PropertyName = "stripNum")]
            public int StripNum { get; set; }

            [JsonProperty(PropertyName = "singleValue")]
            public string SingleValue { get; set; }

            [JsonProperty(PropertyName = "imageType")]
            public ImageTypeEnum ImageType { get; set; }
        }

        #region Private members

        private InspectorSettings settings;

        #endregion

        #region Public Methods

        public VMMicrophone(streamdeck_client_csharp.StreamDeckConnection connection, string action, string context, JObject settings)
        {
            if (settings == null || settings.Count == 0)
            {
                this.settings = InspectorSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = settings.ToObject<InspectorSettings>();
            }

            this.settings.StreamDeckConnection = connection;
            this.settings.ActionId = action;
            this.settings.ContextId = context;
        }

        #endregion

        #region IPluginable

        public void KeyPressed()
        {
            switch (settings.MicType)
            {
                case MicTypeEnum.SingleMode:
                    VMManager.Instance.SetParam(BuildDeviceName(), Convert.ToInt16(settings.SingleValue));
                    break;
                case MicTypeEnum.Toggle:
                    bool isMuted = VMManager.Instance.GetParamBool(BuildDeviceName());
                    VMManager.Instance.SetParam(BuildDeviceName(), isMuted ? 0 : 1);
                    break;
                case MicTypeEnum.PTT:
                    VMManager.Instance.SetParam(BuildDeviceName(), 0);
                    break;
            }
        }

        public void KeyReleased()
        {
            if (settings.MicType == MicTypeEnum.PTT)
            {
                VMManager.Instance.SetParam(BuildDeviceName(), 1);
            }
        }

        public void OnTick()
        {
            settings.SetImageAsync(GetBase64ImageStatus());
        }
      
        public void UpdateSettings(JObject payload)
        {
            if (payload["property_inspector"] != null)
            {
                switch (payload["property_inspector"].ToString().ToLower())
                {
                    case "propertyinspectorconnected":
                        settings.SendToPropertyInspectorAsync();
                        break;

                    case "propertyinspectorwilldisappear":
                        settings.SetSettingsAsync();
                        break;

                    case "updatesettings":
                        settings.MicType     = (MicTypeEnum)Enum.Parse(typeof(MicTypeEnum), (string)payload["micType"]);
                        settings.Strip       = (string)payload["strip"];
                        settings.StripNum    = (int)payload["stripNum"];
                        settings.SingleValue = (string)payload["singleValue"];
                        settings.ImageType   = (ImageTypeEnum)Enum.Parse(typeof(ImageTypeEnum), (string)payload["imageType"]);
                        settings.SetSettingsAsync();
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        private string GetBase64ImageStatus()
        {
            bool isMuted = VMManager.Instance.GetParamBool(BuildDeviceName());
            if (isMuted)
            {
                switch (settings.ImageType)
                {
                    case ImageTypeEnum.Microphone:
                        return Properties.Plugin.Default.MicMute;
                    case ImageTypeEnum.Speaker:
                        return Properties.Plugin.Default.SpeakerDisabled;
                    case ImageTypeEnum.OnOff:
                        return Properties.Plugin.Default.OnOffDisabled;
                    case ImageTypeEnum.Microphone2:
                        return Properties.Plugin.Default.Mic2Mute;
                }

            }
            else
            {
                switch (settings.ImageType)
                {
                    case ImageTypeEnum.Microphone:
                        return Properties.Plugin.Default.MicEnabled;
                    case ImageTypeEnum.Speaker:
                        return Properties.Plugin.Default.SpeakerEnabled;
                    case ImageTypeEnum.OnOff:
                        return Properties.Plugin.Default.OnOffEnabled;
                    case ImageTypeEnum.Microphone2:
                        return Properties.Plugin.Default.Mic2Enabled;
                }
            }

            return Properties.Plugin.Default.MicEnabled;
        }

        private string BuildDeviceName()
        {
            return $"{settings.Strip}[{settings.StripNum}].Mute";
        }

        #endregion
    }
}
