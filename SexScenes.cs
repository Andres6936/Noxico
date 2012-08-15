﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Noxico
{
	class SexScenes
	{
		private static XmlDocument xDoc;
		private static Character top, bottom;

		public static void Engage(Character top, Character bottom, string name = "(starting node)")
		{
			//if (xDoc == null)
			{
				xDoc = new XmlDocument();
				xDoc.LoadXml(Toolkit.ResOrFile(global::Noxico.Properties.Resources.Scenes, "Scenes.xml"));
			}
			SexScenes.top = top;
			SexScenes.bottom = bottom;
			var openings = FindOpenings(name);
			if (openings.Count == 0)
			{
				MessageBox.Message("Could not find a proper opening for scene name \"" + name + "\". Aborting.", true, "Uh-oh.");
				return;
			}
			var scene = openings.FirstOrDefault(i => FiltersOkay(i));
			var message = ApplyTokens(ExtractParagraphsAndScripts(scene));
			var actions = ExtractActions(scene);

			if (top.GetToken("climax").Value >= 100 || bottom.GetToken("climax").Value >= 100)
			{
				actions.Clear();
				if (top.GetToken("climax").Value >= 100 && bottom.GetToken("climax").Value < 100)
				{
					actions.Add("(top climax)", "");
					top.GetToken("climax").Value = 0;
				}
				else if (top.GetToken("climax").Value >= 100 && bottom.GetToken("climax").Value >= 100)
				{
					actions.Add("(both climax)", "");
					top.GetToken("climax").Value = bottom.GetToken("climax").Value = 0;
				}
				else
				{
					actions.Add("(bottom climax)", "");
					bottom.GetToken("climax").Value = 0;
				}
			}

			if (actions.Count == 1)
			{
				var target = actions.First().Key;
				actions.Clear();
				actions.Add(target, "==>");
			}

			if (actions.Count == 0)
				MessageBox.Message(message, true, bottom.Name.ToString(true));
			else
				MessageBox.List(message, actions, () => { Engage(SexScenes.top, SexScenes.bottom, (string)MessageBox.Answer); }, false, true, bottom.Name.ToString(true));
			
			NoxicoGame.HostForm.Noxico.CurrentBoard.Redraw();
			NoxicoGame.HostForm.Noxico.CurrentBoard.Draw();
		}

		private static List<XmlElement> FindOpenings(string sceneName)
		{
			var ret = new List<XmlElement>();
			foreach (var scene in xDoc.SelectNodes("//scene").OfType<XmlElement>().Where(t => t.GetAttribute("name") == sceneName))
				ret.Add(scene);
			return ret;
		}

		private static Dictionary<object, string> ExtractActions(XmlElement scene)
		{
			var ret = new Dictionary<object, string>();
			foreach (var action in scene.SelectNodes("action").OfType<XmlElement>())
			{
				foreach (var s in xDoc.SelectNodes("//scene").OfType<XmlElement>().Where(s => s.GetAttribute("name") == action.GetAttribute("name") && s.HasAttribute("list") && FiltersOkay(s)))
					ret.Add(s.GetAttribute("name"), s.GetAttribute("list"));
			}
			return ret;
		}

		private static string ExtractParagraphsAndScripts(XmlElement scene)
		{
			var ret = new StringBuilder();
			foreach (var part in scene.ChildNodes.OfType<XmlElement>().Where(p => p.Name == "p" || p.Name == "script"))
			{
				if (part.Name == "p")
				{
					ret.AppendLine(part.InnerText.Trim());
					ret.AppendLine();
				}
				else if (part.Name == "script")
				{
					var script = part.InnerText.Replace("$top", top.Name.ToID()).Replace("$bottom", bottom.Name.ToID()).Split('\n');
					var boardchar = NoxicoGame.HostForm.Noxico.CurrentBoard.Entities.OfType<BoardChar>().First(x => x.Character == top);
					boardchar.ScriptRunning = true;
					boardchar.ScriptPointer = 0;
					ret.AppendLine(Noxicobotic.Run(boardchar, script, true).Trim());
					ret.AppendLine();
					boardchar.ScriptRunning = false;
				}
			}
			return ret.ToString();
		}

		private static bool FiltersOkay(XmlElement scene)
		{
			foreach (var filter in scene.ChildNodes.OfType<XmlElement>().Where(f => f.Name == "filter"))
			{
				var fType = filter.GetAttribute("type");
				var fName = filter.GetAttribute("name");
				var fValue = filter.GetAttribute("value");
				var fPrimary = filter.HasAttribute("target") ? (filter.GetAttribute("target") == "top" ? top : bottom) : bottom;
				var fSecondary = fPrimary == top ? bottom : top;
				var fValueF = 0f;
				var fValuePM = '\0';
				if (fValue.EndsWith("+") || fValue.EndsWith("-"))
				{
					fValuePM = fValue[fValue.Length - 1];
					fValue = fValue.Remove(fValue.Length - 1);
				}
				float.TryParse(fValue, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out fValueF);
				switch (fType)
				{
					case "name":
						if (!(fPrimary.Name.ToString(true).Trim().Equals(fName, StringComparison.InvariantCultureIgnoreCase)))
							return false;
						break;
					case "has":
						if (fPrimary.Path(fName) == null)
							return false;
						if (fValueF > 0)
						{
							var num = fPrimary.Tokens.Count(t => t.Name == fName);
							if (fValuePM == '\0')
							{
								if (num != fValueF)
									return false;
							}
							else if (fValuePM == '+')
							{
								if (num < fValueF)
									return false;
							}
							else if (fValuePM == '-')
							{
								if (num > fValueF)
									return false;
							}
						}
						break;
					case "hasnot":
						if (fPrimary.Path(fName) != null)
							return false;
						break;
					case "stat":
					case "value_gteq":
						if (fPrimary.Path(fName).Value < fValueF)
							return false;
						break;
					case "value_equal":
						if (fPrimary.Path(fName).Value != fValueF)
							return false;
						break;
					case "value_lower":
						if (fPrimary.Path(fName).Value >= fValueF)
							return false;
						break;
					case "relation":
						var path = "ships/" + fSecondary.Name + "/" + fValue;
						if (fPrimary.Path(path) == null)
							return false;
						break;
					case "gender":
						if (fValue == "male" && fPrimary.GetGender() != "male")
							return false;
						else if (fValue == "female" && fPrimary.GetGender() != "female")
							return false;
						break;
					case "bodylev":
						var primaryLev = Toolkit.GetLevenshteinString(fPrimary);
						var distance = Toolkit.Levenshtein(primaryLev, NoxicoGame.BodyplanLevs[fValue]);
						if (distance > 0) //?
							return false;
						break;
					case "hasdildo":
						if (!fPrimary.HasToken("items"))
							return false;
						var hasDildo = false;
						foreach (var item in fPrimary.GetToken("items").Tokens)
						{
							var knownItem = NoxicoGame.KnownItems.FirstOrDefault(ki => ki.ID == item.Name);
							if (knownItem == null)
								continue;
							if (knownItem.HasToken("canfuck"))
							{
								if (fValueF > 0)
								{
									var surface = knownItem.GetToken("thickness").Value * knownItem.GetToken("length").Value;
									if (fValuePM == '+' && surface < fValueF)
										continue;
									else if (surface > fValueF)
										continue;
								}
								hasDildo = true;
								break;
							}
						}
						if (!hasDildo)
							return false;
						break;
				}
			}
			return true;
		}

		private static string ApplyTokens(string message)
		{
			var player = NoxicoGame.HostForm.Noxico.Player.Character;
			var tIP = player == top;
			#region Definitions
			var subcoms = new Dictionary<string, Func<Character, string[], string>>()
			{
				{ "You", (c, s) => { return tIP && c == top ? "You" : c.HeSheIt(); } },
				{ "Your", (c, s) => { return tIP && c == top ? "Your" : c.HisHerIts(); } },
				{ "you", (c, s) => { return tIP && c == top ? "you" : c.HeSheIt(true); } },
				{ "your", (c, s) => { return tIP && c == top ? "your" : c.HisHerIts(true); } },

				{ "isme", (c, s) => { return c == player ? s[0] : s[1]; } },
				{ "g", (c, s) => { var g = c.GetGender(); return g == "male" ? s[0] : (g == "hermaphrodite" && s[2] != "" ? s[2] : s[1]); } },
				{ "t", (c, s) => { var t = c.Path(s[0]); return t == null ? "<404>" : t.Text.ToLowerInvariant(); } },
				{ "T", (c, s) => { var t = c.Path(s[0]); return t == null ? "<404>" : t.Text; } },
				{ "v", (c, s) => { var t = c.Path(s[0]); return t == null ? "<404>" : t.Value.ToString(); } },
				{ "l", (c, s) => { var t = c.Path(s[0]); return t == null ? "<404>" : Descriptions.Length(t.Value); } },

				{ "name", (c, s) => { return c.Name.ToString(); } },
				{ "fullname", (c, s) => { return c.Name.ToString(true); } },
				{ "title", (c, s) => { return c.Title; } },
				{ "gender", (c, s) => { return c.GetGender(); } },
				{ "His", (c, s) => { return tIP && c == top ? "Your" : c.HisHerIts(); } },
				{ "He", (c, s) => { return tIP && c == top ? "You" : c.HeSheIt(); } },
				{ "his", (c, s) => { return tIP && c == top ? "your" : c.HisHerIts(true); } },
				{ "he", (c, s) => { return tIP && c == top ? "you" : c.HeSheIt(true); } },
				{ "him", (c, s) => { return tIP && c == top ? "you" : c.HimHerIt(); } },
				{ "is", (c, s) => { return tIP && c == top ? "are" : "is"; } },
				{ "has", (c, s) => { return tIP && c == top ? "have" : "has"; } },
				{ "does", (c, s) => { return tIP && c == top ? "do" : "does"; } },

				{ "hair", (c, s) => { return Descriptions.Hair(c.Path("hair")); } },
				{ "breasts", (c, s) => { if (s[0] == "") s[0] = "0"; return Descriptions.Breasts(c.Path("breastrow[" + s[0] + "]")); } },
				{ "nipple", (c, s) => { if (s[0] == "") s[0] = "0"; return Descriptions.Nipples(c.Path("breastrow[" + s[0] + "]/nipples")); } },
				{ "nipples", (c, s) => { if (s[0] == "") s[0] = "0"; return Descriptions.Nipples(c.Path("breastrow[" + s[0] + "]/nipples")) + 's'; } },
				{ "waist", (c, s) => { return Descriptions.Waist(c.Path("waist")); } },
				{ "hips", (c, s) => { return Descriptions.Hips(c.Path("hips")); } },
				{ "ass", (c, s) => { return Descriptions.Butt(c.Path("ass")); } },
				{ "tail", (c, s) => { return Descriptions.Tail(c.Path("tail")); } },
				//{ "multicock", (c, s) => { return Descriptions.MultiCock(c); } },
				{ "multicock", (c, s) => { return Descriptions.Cock(c.Path("penis[0]")); } },
				{ "cock", (c, s) => { if (s[0] == "") s[0] = "0"; return Descriptions.Cock(c.Path("penis[" + s[0] + "]")); } },
				//{ "pussy", (c, s) => { if (s[0] == "") s[0] = "0"; return Descriptions.Pussy(c.Path("vagina[" + s[0] + "]")); } },
				{ "pussy", (c, s) => { return "pussy"; } },
			};
			#endregion
			#region Parser
			var regex = @"\[ ([A-Za-z]+) (?: \: ([A-Za-z/_]+) )* \]";
			var ro = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;
			while (Regex.IsMatch(message, regex, ro))
			{
				var match = Regex.Match(message, regex, ro);
				var replace = match.ToString();
				var with = "";
				var target = bottom;
				var subcom = "";
				var parms = new List<string>();

				if (!match.Groups[2].Success)
				{
					subcom = match.Groups[1].Value;
				}
				else
				{
					target = (match.Groups[1].Value[0] == 't' ? top : bottom);
					subcom = match.Groups[2].Value;

					if (match.Groups[2].Captures.Count > 1)
					{
						subcom = match.Groups[2].Captures[0].Value;
						foreach (Capture c in match.Groups[2].Captures)
						{
							Console.WriteLine(c);							
							parms.Add(c.Value.Replace('(', '[').Replace(')', ']'));
						}
						parms.RemoveAt(0);
					}
					}

				parms.Add("");
				parms.Add("");
				parms.Add("");

				if (subcoms.ContainsKey(subcom))
					with = subcoms[subcom](target, parms.ToArray());

				message = message.Replace(replace, with);
			}
			#endregion
			#region Old version
			/*
			message = message.Replace("[You]", tIP ? "You" : top.HeSheIt());
			message = message.Replace("[Your]", tIP ? "Your" : top.HisHerIts());
			message = message.Replace("[you]", tIP ? "you" : top.HeSheIt(true));
			message = message.Replace("[your]", tIP ? "you" : top.HisHerIts(true));

			message = message.Replace("[t:name]", top.Name.ToString());
			message = message.Replace("[t:fullname]", top.Name.ToString(true));
			message = message.Replace("[t:title]", top.Title);
			message = message.Replace("[t:gender]", top.GetGender());
			message = message.Replace("[t:His]", tIP ? "Your" : top.HisHerIts());
			message = message.Replace("[t:He]", tIP ? "You" : top.HeSheIt());
			message = message.Replace("[t:his]", tIP ? "your" : top.HisHerIts(true));
			message = message.Replace("[t:he]", tIP ? "you" : top.HeSheIt(true));
			message = message.Replace("[t:him]", tIP ? "you" : top.HimHerIt());
			message = message.Replace("[t:is]", tIP ? "are" : "is");
			message = message.Replace("[t:has]", tIP ? "have" : "has");
			message = message.Replace("[t:does]", tIP ? "do" : "does");
			message = message.Replace("[t:hairdescript]", Descriptions.Hair(top.GetToken("hair")));
			message = message.Replace("[t:breastdescript]", Descriptions.Breasts(top.GetToken("breastrow")));
			message = message.Replace("[t:nippledescript]", Descriptions.Nipples(top.Path("breastrow/nipples")));
			message = message.Replace("[t:waistdescript]", Descriptions.Waist(top.GetToken("waist")));
			message = message.Replace("[t:hipsdescript]", Descriptions.Hips(top.GetToken("hips")));
			message = message.Replace("[t:buttdescript]", Descriptions.Butt(top.GetToken("ass")));
			message = message.Replace("[t:taildescript]", Descriptions.Tail(top.GetToken("tail")));
			message = message.Replace("[t:cockdescript]", Descriptions.Cock(top.GetToken("penis")));
			message = message.Replace("[t:multicockdescript]", Descriptions.Cock(top.GetToken("penis")));

			var freeforms = Regex.Matches(message, @"\[t:(?<you>[^\]]*?)\|(?<him>[^\]]*?)\]");
			foreach (Match match in freeforms)
			{
				var you = match.Groups["you"].ToString();
				var him = match.Groups["him"].ToString();
				message = message.Replace(match.Value, tIP ? you : him);
			}
			freeforms = Regex.Matches(message, @"\[t:g:(?<male>[^\]]*?)\|(?<female>[^\]]*?)(\|(?<herm>[^\]]*?))\]");
			var g = top.GetGender();
			foreach (Match match in freeforms)
			{
				var male = match.Groups["male"].ToString();
				var female = match.Groups["female"].ToString();
				var herm = match.Groups["herm"].ToString();
				message = message.Replace(match.Value, g == "male" ? male : g == "female" && herm != "" ? female : herm);
			}
			freeforms = Regex.Matches(message, @"\[t:(?<textorval>[Ttv]):(?<path>[^\]]*?)\]");
			foreach (Match match in freeforms)
			{
				var path = top.Path(match.Groups["path"].ToString());
				var textorval = match.Groups["textorval"].ToString()[0];
				message = message.Replace(match.Value, path == null ? "<404>" : textorval == 'v' ? path.Value.ToString() : textorval == 'T' ? path.Text : path.Text.ToLowerInvariant());
			}

			message = message.Replace("[b:name]", bottom.Name.ToString());
			message = message.Replace("[b:fullname]", bottom.Name.ToString(true));
			message = message.Replace("[b:title]", bottom.Title);
			message = message.Replace("[b:gender]", bottom.GetGender());
			message = message.Replace("[b:His]", tIP ? "Your" : bottom.HisHerIts());
			message = message.Replace("[b:He]", !tIP ? "You" : bottom.HeSheIt());
			message = message.Replace("[b:his]", !tIP ? "your" : bottom.HisHerIts(true));
			message = message.Replace("[b:he]", !tIP ? "You" : bottom.HeSheIt(true));
			message = message.Replace("[b:him]", !tIP ? "you" : bottom.HimHerIt());
			message = message.Replace("[b:is]", !tIP ? "are" : "is" );
			message = message.Replace("[b:has]", !tIP ? "have" : "has");
			message = message.Replace("[b:does]", !tIP ? "do" : "does");
			message = message.Replace("[b:hairdescript]", Descriptions.Hair(bottom.GetToken("hair")));
			message = message.Replace("[b:breastdescript]", Descriptions.Breasts(bottom.GetToken("breastrow")));
			message = message.Replace("[b:nippledescript]", Descriptions.Nipples(bottom.Path("breastrow/nipples")));
			message = message.Replace("[b:waistdescript]", Descriptions.Waist(bottom.GetToken("waist")));
			message = message.Replace("[b:hipsdescript]", Descriptions.Hips(bottom.GetToken("hips")));
			message = message.Replace("[b:buttdescript]", Descriptions.Butt(bottom.GetToken("ass")));
			message = message.Replace("[b:taildescript]", Descriptions.Tail(bottom.GetToken("tail")));
			message = message.Replace("[b:cockdescript]", Descriptions.Cock(bottom.GetToken("penis")));
			message = message.Replace("[b:multicockdescript]", Descriptions.Cock(bottom.GetToken("penis")));

			freeforms = Regex.Matches(message, @"\[b:(?<you>[^\]]*?)\|(?<him>[^\]]*?)\]");
			foreach (Match match in freeforms)
			{
				var you = match.Groups["you"].ToString();
				var him = match.Groups["him"].ToString();
				message = message.Replace(match.Value, !tIP ? you : him);
			}
			freeforms = Regex.Matches(message, @"\[b:g:(?<male>[^\]]*?)\|(?<female>[^\]]*?)(\|(?<herm>[^\]]*?))\]");
			g = bottom.GetGender();
			foreach (Match match in freeforms)
			{
				var male = match.Groups["male"].ToString();
				var female = match.Groups["female"].ToString();
				var herm = match.Groups["herm"].ToString();
				message = message.Replace(match.Value, g == "male" ? male : g == "female" && herm != "" ? female : herm);
			}
			freeforms = Regex.Matches(message, @"\[b:(?<textorval>[Ttv]):(?<path>[^\]]*?)\]");
			foreach (Match match in freeforms)
			{
				var path = bottom.Path(match.Groups["path"].ToString());
				var textorval = match.Groups["textorval"].ToString()[0];
				message = message.Replace(match.Value, path == null ? "<404>" : textorval == 'v' ? path.Value.ToString() : textorval == 'T' ? path.Text : path.Text.ToLowerInvariant());
			}
			*/
			#endregion
			return message;
		}
	}
}
