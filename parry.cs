if(!isObject(ParryMap)) {
	new ActionMap(ParryMap);
	new ActionMap(ParryMouseMap);
}

function setUpParryMap() {
	ParryMap.bind("keyboard0", "lcontrol", holdParryModifier);
	ParryMap.push();
}

function holdParryModifier(%value) {
	if(%value) {
		ParryMouseMap.bind("mouse0", "button0", parryLeft);
		ParryMouseMap.bind("mouse0", "button1", parryRight);
		ParryMouseMap.bind("keyboard0", "ctrl w", "moveForward");
		ParryMouseMap.bind("keyboard0", "ctrl a", "moveLeft");
		ParryMouseMap.bind("keyboard0", "ctrl s", "moveBackward");
		ParryMouseMap.bind("keyboard0", "ctrl d", "moveRight");
		ParryMouseMap.push();
	}
	else {
		ParryMouseMap.pop();
	}
}

function parryLeft(%value) {
	if(%value) {
		commandToServer('parryLeft');
	}
	else {
		commandToServer('stopParry');
	}
}

function parryRight(%value) {
	if(%value) {
		commandToServer('parryRight');
	}
	else {
		commandToServer('stopParry');
	}
}