datablock AudioProfile(MiniDungeonsSpawnSound) {
	filename    = "./sounds/spawn2.ogg";
	description = Audio2d;
	preload = true;
};

function miniDungeonsHandleSpawn() {
	alxPlay(MiniDungeonsSpawnSound);
	interpolateFOV(170, $pref::Player::defaultFov, 179, 0.025);
}

function interpolateFOV(%start, %end, %current, %speed) {
	%current -= mClampF((%current - %end) * %speed, 0.01, 1000);

	if(%current > %end) {
		PlayGUI.forceFOV = %current;
		$MD::InterpolateFOV = schedule(16, 0, interpolateFOV, %start, %end, %current, %speed);
	}
	else {
		PlayGUI.forceFOV = 0;
		setFov(%end);

		clientCmdCenterPrint("<br><br><br><font:Arial bold:60><color:ffffff>\cp<color:000000>\co\c7THE JOURNEY BEGINS", 3);
	}
}