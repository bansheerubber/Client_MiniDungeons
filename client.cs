schedule(1000, 0, exec, "Add-Ons/Client_MiniDungeons/adjustment.cs");

exec("./profiles/outline.cs");

exec("./healthbar.cs");
exec("./parry.cs");
exec("./welcome.cs");
exec("./potion.cs");

$MDC::Version = "r1";

function clientCmdMD_Handshake() {
	commandToServer('MD_Handshake_Ack', $MDC::Version);

	PlayGUI.add(MD_Healthbar);
	MD_Healthbar.hp = 100;
	MD_Healthbar.maxHp = 100;
	MD_Healthbar.resizeHealthbar();
	MD_Healthbar.ready = true;
}

function formatText(%text, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8) {
	for(%i = 1; %i <= 8; %i++) {
		%text = strReplace(%text, "%" @ %i, %a[%i]);
	}
	return %text;
}