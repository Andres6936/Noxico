var vague = new Tile();
vague.Character = '\u253C';
vague.Foreground = Color.Black;
vague.Background = Color.Black;
var stone = new Tile();
stone.Character = '\u253C';
stone.Foreground = Color.Black;
stone.Background = Color.FromCSS("#4B3027");
var wall = new Tile();
wall.Character = '\u253C';
wall.Foreground = Color.FromCSS("#58343E")
wall.Background = Color.FromCSS("#B34161");
wall.Wall = true;

function BuildPlatform(board, number, vague, stone, wall)
{
	board.MergeBitmap("missions\\portal\\platform" + number + ".png");
	board.Replace("return (tile.Character == 'X' && tile.Foreground.R == 64)", "return vague.Noise()");
	board.Replace("return (tile.Character == 'X' && tile.Foreground.R == 128)", "return stone.Noise()");
	board.Replace("return (tile.Character == 'X' && tile.Foreground.R == 255)", "return wall.Noise()");
}

//Nox side

var myBoard = AddBoard("Portal1");
myBoard.Name = "Demon Portal, Nox Side";
MakeBoardTarget(myBoard);
myBoard.Clear("Desert");
BuildPlatform(myBoard, 1, vague, stone, wall);

var portalC = Character.GetUnique("demonportal_nox");
var portalE = new BoardChar(portalC);
portalE.XPosition = 35;
portalE.YPosition = 10;
portalE.AssignScripts("demonportal_nox");
portalE.ReassignScripts();
myBoard.Entities.Add(portalE);
portalE.ParentBoard = myBoard;
myBoard.DumpToHtml("portal1");


MakeBoardKnown(myBoard); //<-- DEBUG! Should use LearnUnknownLocation("Demon Portal, Nox Side"); in scene to unlock.



//Seradevari side

var vague = new Tile();
vague.Character = ' ';
vague.Foreground = Color.Black;
vague.Background = Color.FromName("Firebrick");
vague.Water = true;

myBoard = AddBoard("Portal2");
myBoard.Name = "Demon Portal, Sera Side";
myBoard.AllowTravel = false;
MakeBoardTarget(myBoard);
myBoard.Clear("Nether");
BuildPlatform(myBoard, 2, vague, stone, wall);

var portalC = Character.GetUnique("demonportal_sera");
var portalE = new BoardChar(portalC);
portalE.XPosition = 35;
portalE.YPosition = 10;
portalE.AssignScripts("demonportal_sera");
portalE.ReassignScripts();
myBoard.Entities.Add(portalE);
portalE.ParentBoard = myBoard;
myBoard.DumpToHtml("portal2");
