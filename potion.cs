if(!isObject(MD_Potion)) {
	exec("./guis/MD_Potion.gui");
}

function MD_Potion::setBar(%this, %sipAmount, %gulpAmount, %remainingHP, %maxHP) {
	MD_Potion.setCentered();
	
	MD_PotionBackground.resize(
		getWord(MD_PotionBackground.position, 0),
		getWord(MD_PotionBackground.position, 1),
		mFloor(MD_PotionBackground.maxWidth * (%remainingHP / %maxHP)),
		getWord(MD_PotionBackground.extent, 1)
	);

	%active = "<color:FFFF00>";
	%disabled = "<color:555555>";
	MD_PotionText.setText(formatText(
		MD_PotionText.format,
		%sipAmount <= %remainingHP ? %active : %disabled,
		%sipAmount,
		%active,
		%gulpAmount <= %remainingHP ? %active : %disabled,
		%gulpAmount
	));
}

function MD_Potion::showUI(%this) {
	PlayGUI.add(MD_Potion);
}

function MD_Potion::hideUI(%this) {
	PlayGUI.remove(MD_Potion);
}

function clientCmdMD_SetPotionBar(%sipAmount, %gulpAmount, %remainingHP, %maxHP) {
	MD_Potion.showUI();
	MD_Potion.setBar(%sipAmount, %gulpAmount, %remainingHP, %maxHP);
}

function clientCmdMD_HidePotionBar() {
	MD_Potion.hideUI();
}

deActivatePackage(MiniDungeonsClientPotion);
package MiniDungeonsClientPotion {
	function disconnect() {
		Parent::disconnect();

		MD_Potion.hideUI();
	}
};
activatePackage(MiniDungeonsClientPotion);