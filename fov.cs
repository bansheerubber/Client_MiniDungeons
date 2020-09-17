function clientCmdMD_interpolateFOV(%end, %current, %speed) {
	if(%end $= "") {
		%end = $pref::Player::defaultFov;
	}

	if(getSubStr(%current, 0, 1) $= "+") {
		%current = %end + getSubStr(%current, 1, 100);
	}
	else if(getSubStr(%current, 0, 1) $= "-") {
		%current -= %end - getSubStr(%current, 1, 100);
	}

	interpolateFOV(%end, %current, %speed, "");
}

function interpolateFOV(%end, %current, %speed, %callback) {
	%current -= mClampF((%current - %end) * %speed, 0.01, 1000);

	if(%current > %end) {
		PlayGUI.forceFOV = %current;
		$MD::InterpolateFOV = schedule(16, 0, interpolateFOV, %end, %current, %speed);
	}
	else {
		PlayGUI.forceFOV = 0;
		setFov(%end);

		if(%callback !$= "") {
			eval(%callback @ "();");
		}
	}
}