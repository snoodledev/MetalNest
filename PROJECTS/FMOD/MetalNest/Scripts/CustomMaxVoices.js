studio.menu.addMenuItem({
name: "Unity\\CustomMaxVoices",
isEnabled: function() {
    var events = studio.window.browserSelection();
    return events
},

execute: function() {
    var events = studio.window.browserSelection();

    var oldVoiceNumber = [];
    var voiceNumber = studio.system.getNumber("Enter the max voices for the selected events", "0");
    var platform = studio.system.getText("Enter the Platform", "Switch");

    for (x = 0; x < events.length; x++) {

        oldVoiceNumber[x] = events[x].automatableProperties.maxVoices
        events[x].automatableProperties.maxVoices = voiceNumber;
    }

    studio.project.build({
        platforms: platform
    });

    for (x = 0; x < events.length; x++) {
        events[x].automatableProperties.maxVoices = oldVoiceNumber[x];
    }
}});