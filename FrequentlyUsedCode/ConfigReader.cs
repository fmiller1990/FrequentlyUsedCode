using System;
using System.Collections.Generic;

namespace FrequentlyUsedCode
{

    /// <summary>
    /// Liest ConfigFiles aus. Syntax wie in ConfigBeispiel.txt
    /// </summary>
    public class ConfigReader
    {
        private string path;
        private Dictionary<string, Dictionary<string, string>> sections;

        /// <summary>
        /// Constructor fuer ConfigReader Instanz.
        /// </summary>
        /// <param name="path">Voll Qualifizierter Pfad für Configdatei.</param>
        public ConfigReader(string path) {
            this.path = path;
            sections = new Dictionary<string, Dictionary<string, string>>();
        }


        /// <summary>
        /// Liest Datei aus. Werte werden in this.sections gespeichert.
        /// <exception cref="KeyNotFoundException">Fuer einen Schluessel gibt es keinen Wert.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Datei in path existiert nicht oder ist nicht lesbar.</exception>
        /// <exception cref="Exception">Andere Exception.</exception>
        /// </summary>
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
                                    throw new KeyNotFoundException($"Die Einstellung Key = {current_key} hat keinen zugeordneten Wert. \nIn {path}:line {lineNum}.");
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
                throw new System.IO.FileNotFoundException($"Die Konfigurationsdatei {path} wurde nicht gefunden. \nOriginal Exception: {ex}");
            }
            // Andere Fehler
            catch (Exception ex) {
                throw new Exception($"Die Konfig-Datei ist nicht korrekt lesbar. \nDatei = {path} \nZeile = {lineNum} \nSektion = {current_section} \nKey = {current_key} \nValue = {current_value}. \nOriginale Exception: {ex}");
            }
        }


        /// <summary>
        /// Gibt den Wert aus der Sektion zurück.
        /// </summary>
        /// <param name="section">Die Sektion, in der gesucht wird.</param>
        /// <param name="key">Schluessel, dessen Wert gesucht wird.</param>
        /// <exception cref="KeyNotFoundException">Wert fuer diesen Key existiert nicht.</exception>
        /// <returns></returns>
        public string GetValue(string section, string key) {
            try {
                return sections[section.ToUpper()][key];
            }
            catch (KeyNotFoundException ex) {
                throw new KeyNotFoundException($"Konfiguration für dieses Hostprofile konnte nicht gefunden werden. \nDatei = {path} \nEinstellung = {key} \nSektion = {section}. \nOriginal Exception: {ex}");
            }
        }
    }

    class RunConfigReader
    {
        /// <summary>
        /// Minimales Nutzungsbeispiel fuer ConfigReader.
        /// </summary>
        public static void MinimalExample() {
            try {
                //Usecase 1: einfache KeyValuePairs

                //Initialisiere Instanz
                ConfigReader configReaderInstanz = new ConfigReader("..\\..\\ConfigUsecase1.txt");
                //Lese ConfigDatei
                configReaderInstanz.Read();
                //Lese einen Wert aus. Format ist implizit string.
                Console.WriteLine(configReaderInstanz.GetValue("TestSektion", "TestKey"));
                //Als Bool.
                Console.WriteLine(Convert.ToBoolean(configReaderInstanz.GetValue("TestSektion", "TestBool")));
                //Als Int.
                Console.WriteLine(Convert.ToInt32(configReaderInstanz.GetValue("TestSektion", "TestInt")));
            }
            catch (Exception ex) {
                //Exceptions werden von ConfigReader weitergegeben.
                throw ex;
            }

            try {
                //Usecase 2: Externe Konfig mit Rechnern in Gruppen, Gruppen haben eigenschaften.

                //Initialisiere Instanz
                ConfigReader dynamicReader = new ConfigReader("..\\..\\ConfigUsecase2.txt");
                //Lese ConfigDatei
                dynamicReader.Read();
                //Finde die Gruppe, in der "TestName" enthalten ist.
                string group = dynamicReader.GetValue("Gruppen", "TestName");
                //Lese aus dieser Gruppe einen Wert aus. Fuer mehr Typen siehe Usecase 1.
                Console.WriteLine(dynamicReader.GetValue(group, "TestKey"));
                return;
            }
            catch (Exception ex) {
                //Exceptions werden von ConfigReader weitergegeben.
                throw ex;
            }
        }
    }
}
