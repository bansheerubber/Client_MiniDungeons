function clientCmdMD_HandleSpawn() {
	miniDungeonsHandleSpawn();
}

function miniDungeonsHandleSpawn() {
	if(!isObject(MiniDungeonsSpawnSound)) {
		datablock AudioProfile(MiniDungeonsSpawnSound) {
			filename    = "Add-Ons/Client_MiniDungeons/sounds/spawn2.ogg";
			description = Audio2d;
			preload = true;
		};
	}
	
	alxPlay(MiniDungeonsSpawnSound);
	interpolateFOV($pref::Player::defaultFov, 168, 0.025, "MD_displayWelcomeMessage");
}

function MD_displayWelcomeMessage() {
	clientCmdCenterPrint("<br><br><br><font:Arial bold:60><color:ffffff>\cp<color:000000>\co\c7THE JOURNEY BEGINS", 3);
}