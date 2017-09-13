using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading;
using System.Text.RegularExpressions;

namespace AlphaCodex
{
    public partial class MainWindow : Window
    {
        private void AddSpellPanel_Loaded(object sender, System.EventArgs e)
        {
        }



        // controls
        private void RegisterSpell()
        {
            string Input;

            Input = "Animal Trance\nEnchantment(Compulsion)[Mind -\nAffecting, Sonic]\nLevel: Brd 2, Drd 2\nComponents: V, S\nCasting Time: 1 standard action\nRange: Close(25 ft. + 5 ft./ 2 levels)\nTargets: Animals or magical beasts with\nIntelligence 1 or 2\nDuration: Concentration\nSaving Throw: Will negates; see text\nSpell Resistance: Yes\nYour swaying motions and music(or.";

            //Console.WriteLine(Input);
            Regex rex = new Regex(@"([^\n]+)\n(.+)\nLevel: (.+)\nComponents: (.+)\nCasting Time: (.+)\nRange: (.+)\n(Targets?:)?(Effect:)?(Area:)? (.+)\nDuration: (.+)\nSaving Throw: (.+)\nSpell Resistance: ([^\n]+)(.+)", RegexOptions.Singleline);
            Match match = rex.Match(Input);
            if (match != null)
            {
                GroupCollection groups = match.Groups;

                var Name = groups[1].Value;
                Console.WriteLine("[Spell][Name]{0}", Name);
                var School = groups[2].Value.Replace('\n', ' ');
                //get all the school info: school_name(subschool_name)/other_school_name(other_subschool_name) [descriptor, descriptor, ...]
                Console.WriteLine("[Spell][School]{0}", School);
                var Levels = groups[3].Value.Replace('\n', ' ');
                //get the spell level for each class, domain. Can be Brd 4, Sor/Wiz 4 or Bard 4, Sorcerer/Wizard 4, Death
                //Names of spellcasting classes are abbreviated as follows: bard Brd;
                //cleric Clr; druid Drd; paladin Pal; ranger Rgr; sorcerer Sor; wizard Wiz.
                Console.WriteLine("[Spell][Level]{0}", Levels);
                var Components = groups[4].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Components]{0}", Components);
                // Parse, possible values: V, S, F, M, DF, F/DF, XP, M/DF
                var CastingTime = groups[5].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Casting Time]{0}", CastingTime);
                var Range = groups[6].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Range]{0}", Range);
                for (int i = 7; i < 10; i++)
                {
                    if (groups[i].Value != "")
                    {
                        var AimingType = groups[i].Value.Replace('\n', ' ');
                        Console.WriteLine("[Spell][Aiming Type]{0}", AimingType);
                        break;
                    }
                }
                var AimingText = groups[10].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Aiming Text]{0}", AimingText);
                var Duration = groups[11].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Duration]{0}", Duration);
                var SavingThrow = groups[12].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Saving Throw]{0}", SavingThrow);
                var SpellResistance = groups[13].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Spell Resistance]{0}", SpellResistance);
                // booleans (yes/no) + (object) + (harmless) or text ?
                var DescriptiveText = groups[14].Value.Replace('\n', ' ');
                Console.WriteLine("[Spell][Descriptive Text]{0}", DescriptiveText);
                // should filter material components/focus/xp/etc ?
            }
        }
    }
}