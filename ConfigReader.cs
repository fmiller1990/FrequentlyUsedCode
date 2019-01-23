using System;
using System.Collections.Generic;

namespace BakeControl
{
    public class ConfigReader
    {
        private string path;
        private Dictionary<string, Dictionary<string, string>> sections;

        public ConfigReader(string path) {
            this.path = path;
            sections = new Dictionary<string, Dictionary<string, string>>();
        }

        public void Read() {
            string line;
            string current_section = "";
            string current_key = "";
            string current_value = "";
            string[] current_keyvaluepair;
            int lineNum = 0;
            try {
                System.IO.StreamReader file = new System.IO.StreamReader(path);

                while ((line = file.ReadLine()) != null) {
                    lineNum++;
                    line = line.Trim();
                    if (line != "" && line.StartsWith(";") == false && line.StartsWith("#") == false) {
                        if (line.StartsWith("[") && line.EndsWith("]")) {
                            current_section = line.Substring(1, line.Length - 2).ToUpper();
                            sections.Add(current_section, new Dictionary<string, string>());
                            continue;
                        }
                        else {
                            current_keyvaluepair = line.Split(new char[] { '=' }, 2);
                            current_key = current_keyvaluepair[0].Trim();
                            if (current_keyvaluepair.Length == 2) {
                                current_value = current_keyvaluepair[1].Trim();
                                if (current_value == null || current_value == "") {
                                    Global.Globals.ShowError($"Die Einstellung Key = {current_key} hat keinen zugeordneten Wert. \nIn {path}:line {lineNum}.");
                                }
                            }
                        }
                        sections[current_section].Add(current_key, current_value);
                    }
                }
                file.Close();
            }
            // Nur wenn die Datei nicht gefunden wurde.
            catch (Exception ex) when 
            (ex is System.IO.FileNotFoundException || ex is System.IO.DirectoryNotFoundException) {
                Global.Globals.ShowError($"Die Konfigurationsdatei {path} wurde nicht gefunden.");
                throw;
            }
            // Andere Fehler
            catch (Exception ex) {
                Global.Globals.ShowError($"Die Konfig-Datei ist nicht korrekt lesbar. \nDatei = {path} \nZeile = {lineNum} \nSektion = {current_section} \nKey = {current_key} \nValue = {current_value}");
                throw ex;
            }
        }

        public string GetValue(string section, string key) {
            try {
                return sections[section.ToUpper()][key];
            }
            catch (KeyNotFoundException) {
                Global.Globals.ShowError($"Konfiguration für dieses Hostprofile konnte nicht gefunden werden. \nDatei = {path} \nEinstellung = {key} \nSektion = {section}");
                return null;
            }
        }
    }
}
