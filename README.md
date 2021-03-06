# streamdeck-voicemeeter
VoiceMeeter integration and live feedback for the Elgato Stream Deck device.

## What's new in v1.0
### 1.
VoiceMeeter Advanced Plugin is now called *VoiceMeeter Advanced Click/Long-Click* to differentiate from the brand new *VoiceMeeter Advanced Toggle* plugin

### 2.
#### Changes to "VoiceMeeter Mute/Unmute":
- You can now choose your own images to display, instead of the 4 pre-defined icons

### 3.
#### "VoiceMeeter Advanced Toogle"
- **Brand new plugin!**
- Read below for full features list

## Current functionality
### 4 Plugins built into one:
#### VoiceMeeter Mute/Unmute
- Allows you to easily connect to one of VoiceMeeter's Strips or Buses
- 3 different modes: Toggle/Push-To-Talk/Single Setting (on/off)
- See a live indication of the current status on Stream Deck (never forget your microphone on again!)
- Can also be used to mute/unmute different Strips/Buses such as Spotify/Background music/etc.
- Choose from 4 different icons to display the mute/unmute settings
- **New in v1.0:** You can now choose your own images to display, instead of the 4 pre-defined icons

#### VoiceMeeter Modify Setting
- Allows you to easily modify various VoiceMeeter settings
- Supports a whole list of options for each Strip/Bus
	* Options include modifying the: Gain slider, gate, comp,  mono button, solo button, audibility, color_x, color_y, eqgain1, eqgain2, eqgain3, fx_x, fx_y, mc,pan_x, pan_y
	(Valid values can be found starting on page 9 of VoiceMeeter API PDF: https://download.vb-audio.com/Download_CABLE/VoicemeeterRemoteAPI.pdf )
- Live feedback on the current value of that setting
- Supports both Click and Long Click (allows you to toggle between two preset values for this setting)
- Option to turn off the Live feedback and set the title to whatever you want

#### VoiceMeeter Advanced Click/Long-Click
- **Note:** This is for advanced users (you better know what you're doing)
- Allows you to directly modify a whole set of settings
- Example: `Strip[0].mono=1;Strip[1].Mute=1;Bus[2].Gain=-20;`
	* Additional examples can be found on the VoiceMeeter forum: https://forum.vb-audio.com/viewtopic.php?f=8&t=346&sid=a773394396c10847fd6fd7e332a55e49#p495
	and the VoiceMeeter API PDF: https://download.vb-audio.com/Download_CABLE/VoicemeeterRemoteAPI.pdf
- Supports both Click and Long Click (allows you to change between two preset list of settings)
- Live feedback on whatever setting you choose
- Option to turn off the Live feedback and set the title to whatever you want

#### VoiceMeeter Advanced Toogle
- **Note:** This is for advanced users (you better know what you're doing)
- Focused on toggling between two modes (versus click and long click in the VoiceMeeter Advanced Click/Long-Click)
- Example: `Strip[0].mono=1;Strip[1].Mute=1;Bus[2].Gain=-20;`
	* Additional examples can be found on the VoiceMeeter forum: https://forum.vb-audio.com/viewtopic.php?f=8&t=346&sid=a773394396c10847fd6fd7e332a55e49#p495
	and the VoiceMeeter API PDF: https://download.vb-audio.com/Download_CABLE/VoicemeeterRemoteAPI.pdf
- Supports toggling between two preset list of settings
- Supports different user-defined icons for each preset
- Live feedback on whatever setting you choose
- Option to turn off the Live feedback and set the title to whatever you want

##### Advanced fields explained:
- Mode1 Key Press - The configuration to set when we're toggling into Mode1 -> Use this to turn ON the setting e.g. `Strip[0].Mute=1`
- Mode1 Check - True/False value to determine if we're in Mode1. Example: If you input: `Strip[0].Mute` the plugin will determine you're in Mode1 every time Strip0 is muted.
- Mode1 Image - Customizable image that will be shown when you're in Mode1
- Mode2 Key Press - The configuration to set when we're toggling into Mode2 -> Use this to turn OFF the setting e.g. `Strip[0].Mute=0`
- Mode2 Key Press - Customizable image that will be shown when you're in Mode1
- Title - Used to determine if you want a dynamic title (Based on the "Title Value" field) or a static title (Based on the "Title field" at the very top)
- Title Value - Value to display in the title. Example: If you input: `Strip[0].Mono` it will display `1` when Mono is enabled on Strip0 and `0` otherwise.


### FAQ
Q: Can I use this plugin to Restart VoiceMeeter?  
A: Yes! Choose one of the "VoiceMeeter Advanced" plugins and use the following command: `Command.Restart = 1;`

Q: Stream Deck shows a big VoiceMeeter logo and nothing works  
A: This means that VoiceMeeter is either not running or not properly installed. Try reloading VoiceMeeter, if that doesn't work - try reinstalling.

Q: What are the valid values for each setting?  
A: Valid values can be found starting on page 9 of VoiceMeeter API PDF: https://download.vb-audio.com/Download_CABLE/VoicemeeterRemoteAPI.pdf

## Dependencies
- This library uses the [streamdeck-client-csharp](https://github.com/TyrenDe/streamdeck-client-csharp) wrapper
- This library uses a modified version of [VoiceMeeterWrapper](https://github.com/tocklime/VoiceMeeterWrapper) and includes additional functionality

## Feature roadmap
Always open to more suggestions.

## How do I get started using it?
Install by clicking the com.barraider.voicemeeter.streamDeckPlugin file in the Releases folder:
https://github.com/BarRaider/streamdeck-voicemeeter/releases

## I found a bug, who do I contact?
Just head over to the issues page and create a new issue.

## License
MIT License

Copyright (c) 2019

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
