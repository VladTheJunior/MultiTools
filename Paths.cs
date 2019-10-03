#region copyright
/*MIT License

Copyright (c) 2015-2017 XaKO

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows;
using System.Security.Cryptography;
using System.Xml;
using System.Linq;

namespace ESO_Assistant.Classes
{
    public class Paths
    {

        public static void OpenESOA()
        {
            try
            {
                using (RegistryKey AS = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ESO-Assistant"))
                {
                    object P = AS.GetValue("Path");

                    if (P != null)
                    {
                        Process process = new Process()
                        {
                            StartInfo = new ProcessStartInfo(Path.Combine(P.ToString(), "ESO-Assistant.exe"))
                            {
                                WorkingDirectory = P.ToString()
                            }
                        };

                        process.Start();
                    }
                    else
                        MessageBox.Show((string)Application.Current.FindResource("ESO-Assistant not found!"));
                }
            }
            catch
            {
                MessageBox.Show((string)Application.Current.FindResource("ESO-Assistant not found!"));
            }
        }

        public static void OpenTAD(string Name)
        {
            if (Process.GetProcessesByName("age3").Length != 0 || Process.GetProcessesByName("age3y").Length != 0 || Process.GetProcessesByName("age3p").Length != 0 || Process.GetProcessesByName("age3t").Length != 0 || Process.GetProcessesByName("age3x").Length != 0 || Process.GetProcessesByName("age3xpmod").Length != 0)
                MessageBox.Show((string)Application.Current.FindResource("The game is already running!"));
            else
            {
                try
                {
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            Process process = new Process()
                            {
                                StartInfo = new ProcessStartInfo(Path.Combine(P.ToString(), Name))
                                {
                                    WorkingDirectory = P.ToString()
                                }
                            };

                            process.Start();
                        }
                        else
                            MessageBox.Show((string)Application.Current.FindResource("The game is not found!"));
                    }
                }
                catch
                {
                    MessageBox.Show((string)Application.Current.FindResource("The game is not found!"));
                }
            }
        }


        public static string getGamePath()
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                return P.ToString();
            }

        }


        public static Visibility IsEPInstalled()
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (File.Exists(Path.Combine(P.ToString(), "age3f.exe")))
                        return Visibility.Visible;

                }
            }
            return Visibility.Collapsed;
        }


        public static string GetEPVersion()
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (File.Exists(Path.Combine(P.ToString(), "age3f.exe")))
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(P.ToString(), "age3f.exe"));
                        return versionInfo.FileVersion.Split('.').Last();
                    }

                    }
            }
            return null;
        }



        public static Visibility IsTPInstalled()
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (File.Exists(Path.Combine(P.ToString(), "age3t.exe")))
                        return Visibility.Visible;

                }
            }
            return Visibility.Collapsed;
        }


        public static Visibility IsXPInstalled()
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (File.Exists(Path.Combine(P.ToString(), "age3xpmod.exe")))
                        return Visibility.Visible;

                }
            }
            return Visibility.Collapsed;
        }

        public static void OpenTWC(string Name)
        {
            if (Process.GetProcessesByName("age3").Length != 0 || Process.GetProcessesByName("age3y").Length != 0 || Process.GetProcessesByName("age3p").Length != 0 || Process.GetProcessesByName("age3t").Length != 0 || Process.GetProcessesByName("age3x").Length != 0 || Process.GetProcessesByName("age3xpmod").Length != 0)
                MessageBox.Show((string)Application.Current.FindResource("The game is already running!"));
            else
            {
                try
                {
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            Process process = new Process()
                            {
                                StartInfo = new ProcessStartInfo(Path.Combine(P.ToString(), Name))
                                {
                                    WorkingDirectory = P.ToString()
                                }
                            };

                            process.Start();
                        }
                        else
                            MessageBox.Show((string)Application.Current.FindResource("The game is not found!"));
                    }
                }
                catch
                {
                    MessageBox.Show((string)Application.Current.FindResource("The game is not found!"));
                }
            }
        }
        public static void OpenNilla(string Name)
        {
            if (Process.GetProcessesByName("age3").Length != 0 || Process.GetProcessesByName("age3y").Length != 0 || Process.GetProcessesByName("age3p").Length != 0 || Process.GetProcessesByName("age3t").Length != 0 || Process.GetProcessesByName("age3x").Length != 0 || Process.GetProcessesByName("age3xpmod").Length != 0)
                MessageBox.Show((string)Application.Current.FindResource("The game is already running!"));
            else
            {
                try
                {
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            Process process = new Process()
                            {
                                StartInfo = new ProcessStartInfo(Path.Combine(P.ToString(), Name))
                                {
                                    WorkingDirectory = P.ToString()
                                }
                            };

                            process.Start();
                        }
                        else
                            MessageBox.Show((string)Application.Current.FindResource("The game is not found!"));
                    }
                }
                catch
                {
                    MessageBox.Show((string)Application.Current.FindResource("The game is not found!"));
                }
            }
        }


        private static bool isMD5Equal(string Path, string MD5)
        {
            bool Result = false;
            if (File.Exists(Path))
                if (MD5 == GetMD5(Path))
                    Result = true;
            return Result;
        }

        private static void DeleteMD5Equal(string Path, string MD5)
        {
            if (File.Exists(Path))
                if (MD5 == GetMD5(Path))
                    File.Delete(Path);
        }

        public static string GetMD5(string FileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(FileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty).ToLower();
                }
            }
        }

        public static bool isEkantaInstalled()
        {
            bool Result = false;
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (isMD5Equal(Path.Combine(P.ToString(), "Data", "uiarmybanners.xml.xmb"), "c45e0a6db8e61f985cd8a7cfca4503a6") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnew.xml.xmb"), "9436be30409630b3d0d51498ae24757e") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnewmin.xml.xmb"), "bc60027969c6af5d3d6dc3d01ec2ad58") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitycommandpanel.xml.xmb"), "c88debfa6017392163b58e65cac6c338") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitydeck.xml.xmb"), "6a01312bfd9f8eaec3f30c99b36e4aa7") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitytransportpanelheader.xml.xmb"), "2d598cefc7b2e3f9454004f2015d302c") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uimainnew.xml.xmb"), "7c8145ce794fdcf7e5831a4b810a3b81") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnew.xml.xmb"), "02f7731072c3afe8e40cc2fcf1b7f7c8") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnewmin.xml.xmb"), "ec50333c28d9a772366cccd2280f5403") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2.xml.xmb"), "e9aa7a803616b1714a71c3a498202c06") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2min.xml.xmb"), "2ab34d05819233e57ff05f0bc0360412") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiplayersummarydlg.xml.xmb"), "4f49a13914cb2640bf979eeeeb727429") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uipostgamescreen.xml.xmb"), "b0887df11026627732bde4e50580f4dd") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2.xml.xmb"), "e66f1f86be009f5e585e4ccebb29cad5") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2min.xml.xmb"), "cf4ae9c200400981b20e589842d2399f") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontains.xml.xmb"), "526483d91ef904d424a82a05af0fb44b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontainsmin.xml.xmb"), "526483d91ef904d424a82a05af0fb44b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselection.xml.xmb"), "4dbfb511f8a9b079bb2bfdb92970ade5") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselectionmin.xml.xmb"), "464191859a843dbc1930567c3b64e14a") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3.xml.xmb"), "f1424b00d17efba64de795cd08d11ced") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3min.xml.xmb"), "ab8b7ff1dfc9271be4aff54e2840bfaa") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanel.xml.xmb"), "41a7e6e5f6dc48b92704c6bbce34127b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanelmin.xml.xmb"), "41a7e6e5f6dc48b92704c6bbce34127b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanel.xml.xmb"), "e1fad357fe7732760ed6742af5cbb8e5") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanelmin.xml.xmb"), "38a79b2ebcb9c24eed42e4ded42279d3") &&

                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_active_ypack.ddt"), "bf93a0b74545b2bd1176b7cf381d542b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_depressed_ypack.ddt"), "33f075fb02b9b706359074935b3f2240") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_normal_ypack.ddt"), "141f6f3943c712517748b775e94c9e9f") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_active_ypack.ddt"), "a3aa314cf0f3500f24476e0bf4015742") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_depressed_ypack.ddt"), "a295c0a43e1b9182dd45609033a6235a") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_normal_ypack.ddt"), "affebc408f675fefd29e54e649c85f33") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_active_ypack.ddt"), "c2be69a8ae6d622a88b0f2b043d91d8d") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_depressed_ypack.ddt"), "bb9689bcfd6841fa5a584443c64b23fe") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_normal_ypack.ddt"), "363b4269fe27c5a703cd71d660787416") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "Resource.ddt"), "926b480655d8700aa08128e331c64db6") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "Select.ddt"), "305d184a0346164da2aee67107b621e9")

                        )
                        Result = true;
                }

            }
            return Result;
        }

        public static void RemoveEkantaIfInstalled()
        {
            if (isEkantaInstalled())
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiarmybanners.xml.xmb"), "c45e0a6db8e61f985cd8a7cfca4503a6");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnew.xml.xmb"), "9436be30409630b3d0d51498ae24757e");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnewmin.xml.xmb"), "bc60027969c6af5d3d6dc3d01ec2ad58");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitycommandpanel.xml.xmb"), "c88debfa6017392163b58e65cac6c338");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitydeck.xml.xmb"), "6a01312bfd9f8eaec3f30c99b36e4aa7");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitytransportpanelheader.xml.xmb"), "2d598cefc7b2e3f9454004f2015d302c");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uimainnew.xml.xmb"), "7c8145ce794fdcf7e5831a4b810a3b81");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnew.xml.xmb"), "02f7731072c3afe8e40cc2fcf1b7f7c8");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnewmin.xml.xmb"), "ec50333c28d9a772366cccd2280f5403");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2.xml.xmb"), "e9aa7a803616b1714a71c3a498202c06");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2min.xml.xmb"), "2ab34d05819233e57ff05f0bc0360412");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiplayersummarydlg.xml.xmb"), "4f49a13914cb2640bf979eeeeb727429");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uipostgamescreen.xml.xmb"), "b0887df11026627732bde4e50580f4dd");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2.xml.xmb"), "e66f1f86be009f5e585e4ccebb29cad5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2min.xml.xmb"), "cf4ae9c200400981b20e589842d2399f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontains.xml.xmb"), "526483d91ef904d424a82a05af0fb44b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontainsmin.xml.xmb"), "526483d91ef904d424a82a05af0fb44b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselection.xml.xmb"), "4dbfb511f8a9b079bb2bfdb92970ade5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselectionmin.xml.xmb"), "464191859a843dbc1930567c3b64e14a");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3.xml.xmb"), "f1424b00d17efba64de795cd08d11ced");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3min.xml.xmb"), "ab8b7ff1dfc9271be4aff54e2840bfaa");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanel.xml.xmb"), "41a7e6e5f6dc48b92704c6bbce34127b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanelmin.xml.xmb"), "41a7e6e5f6dc48b92704c6bbce34127b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanel.xml.xmb"), "e1fad357fe7732760ed6742af5cbb8e5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanelmin.xml.xmb"), "38a79b2ebcb9c24eed42e4ded42279d3");

                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_active_ypack.ddt"), "bf93a0b74545b2bd1176b7cf381d542b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_depressed_ypack.ddt"), "33f075fb02b9b706359074935b3f2240");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_normal_ypack.ddt"), "141f6f3943c712517748b775e94c9e9f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_active_ypack.ddt"), "a3aa314cf0f3500f24476e0bf4015742");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_depressed_ypack.ddt"), "a295c0a43e1b9182dd45609033a6235a");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_normal_ypack.ddt"), "affebc408f675fefd29e54e649c85f33");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_active_ypack.ddt"), "c2be69a8ae6d622a88b0f2b043d91d8d");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_depressed_ypack.ddt"), "bb9689bcfd6841fa5a584443c64b23fe");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_normal_ypack.ddt"), "363b4269fe27c5a703cd71d660787416");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "Resource.ddt"), "926b480655d8700aa08128e331c64db6");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "Select.ddt"), "305d184a0346164da2aee67107b621e9");


                    }

                }
        }

        public static bool isQazInstalled()
        {
            bool Result = false;
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (isMD5Equal(Path.Combine(P.ToString(), "Data", "uiarmybanners.xml.xmb"), "f6d250eacb80384c21f243b9629f86e7") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnew.xml.xmb"), "0f5fbb960c0180d119d6df85ae70ecae") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnewmin.xml.xmb"), "0f5fbb960c0180d119d6df85ae70ecae") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitycommandpanel.xml.xmb"), "a3daed427862ba6c6ddf5bde38cc9d87") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitydeck.xml.xmb"), "3b5eb24744b8253d3b68b8310b673864") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uimainnew.xml.xmb"), "75cf87273ab6d35a859d2aeef8cb683d") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnew.xml.xmb"), "585c7fc7ad311bd6686690033c0c825f") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnewmin.xml.xmb"), "585c7fc7ad311bd6686690033c0c825f") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2.xml.xmb"), "e9aa7a803616b1714a71c3a498202c06") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2min.xml.xmb"), "e9aa7a803616b1714a71c3a498202c06") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiplayersummarydlg.xml.xmb"), "9500a4990ff3b7b56049539539ffd957") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2.xml.xmb"), "e66f1f86be009f5e585e4ccebb29cad5") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2min.xml.xmb"), "e66f1f86be009f5e585e4ccebb29cad5") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontains.xml.xmb"), "b7bf583154bf26a5c66c4784349538f0") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontainsmin.xml.xmb"), "b7bf583154bf26a5c66c4784349538f0") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselection.xml.xmb"), "affbcc7dc9a49e0a6320207d086723a4") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselectionmin.xml.xmb"), "c1cde947c7283123de1a853beba9ea36") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3.xml.xmb"), "f1424b00d17efba64de795cd08d11ced") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3min.xml.xmb"), "0af2971b23683bfb4f39be9b716f8008") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanel.xml.xmb"), "9b7b5bb8c182c7fae349d69ac1b3fed3") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanelmin.xml.xmb"), "9b7b5bb8c182c7fae349d69ac1b3fed3") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanel.xml.xmb"), "01fe6bf3fdab13a060b4b66075a625b5") &&
                        isMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanelmin.xml.xmb"), "23f770e5f66ea7ffd9f7d859b5799d80") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_active_ypack.ddt"), "bf93a0b74545b2bd1176b7cf381d542b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_depressed_ypack.ddt"), "33f075fb02b9b706359074935b3f2240") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_normal_ypack.ddt"), "141f6f3943c712517748b775e94c9e9f") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_active_ypack.ddt"), "a3aa314cf0f3500f24476e0bf4015742") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_depressed_ypack.ddt"), "a295c0a43e1b9182dd45609033a6235a") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_normal_ypack.ddt"), "affebc408f675fefd29e54e649c85f33") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_active_ypack.ddt"), "c2be69a8ae6d622a88b0f2b043d91d8d") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_depressed_ypack.ddt"), "bb9689bcfd6841fa5a584443c64b23fe") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_normal_ypack.ddt"), "363b4269fe27c5a703cd71d660787416")
                        )

                        Result = true;
                }
            }
            return Result;
        }
        public static void RemovesQazIfInstalled()
        {
            if (isQazInstalled())
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiarmybanners.xml.xmb"), "f6d250eacb80384c21f243b9629f86e7");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnew.xml.xmb"), "0f5fbb960c0180d119d6df85ae70ecae");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnewmin.xml.xmb"), "0f5fbb960c0180d119d6df85ae70ecae");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitycommandpanel.xml.xmb"), "a3daed427862ba6c6ddf5bde38cc9d87");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitydeck.xml.xmb"), "3b5eb24744b8253d3b68b8310b673864");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uimainnew.xml.xmb"), "75cf87273ab6d35a859d2aeef8cb683d");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnew.xml.xmb"), "585c7fc7ad311bd6686690033c0c825f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnewmin.xml.xmb"), "585c7fc7ad311bd6686690033c0c825f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2.xml.xmb"), "e9aa7a803616b1714a71c3a498202c06");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2min.xml.xmb"), "e9aa7a803616b1714a71c3a498202c06");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiplayersummarydlg.xml.xmb"), "9500a4990ff3b7b56049539539ffd957");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2.xml.xmb"), "e66f1f86be009f5e585e4ccebb29cad5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2min.xml.xmb"), "e66f1f86be009f5e585e4ccebb29cad5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontains.xml.xmb"), "b7bf583154bf26a5c66c4784349538f0");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontainsmin.xml.xmb"), "b7bf583154bf26a5c66c4784349538f0");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselection.xml.xmb"), "affbcc7dc9a49e0a6320207d086723a4");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselectionmin.xml.xmb"), "c1cde947c7283123de1a853beba9ea36");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3.xml.xmb"), "f1424b00d17efba64de795cd08d11ced");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3min.xml.xmb"), "0af2971b23683bfb4f39be9b716f8008");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanel.xml.xmb"), "9b7b5bb8c182c7fae349d69ac1b3fed3");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanelmin.xml.xmb"), "9b7b5bb8c182c7fae349d69ac1b3fed3");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanel.xml.xmb"), "01fe6bf3fdab13a060b4b66075a625b5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanelmin.xml.xmb"), "23f770e5f66ea7ffd9f7d859b5799d80");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_active_ypack.ddt"), "bf93a0b74545b2bd1176b7cf381d542b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_depressed_ypack.ddt"), "33f075fb02b9b706359074935b3f2240");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_normal_ypack.ddt"), "141f6f3943c712517748b775e94c9e9f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_active_ypack.ddt"), "a3aa314cf0f3500f24476e0bf4015742");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_depressed_ypack.ddt"), "a295c0a43e1b9182dd45609033a6235a");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_normal_ypack.ddt"), "affebc408f675fefd29e54e649c85f33");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_active_ypack.ddt"), "c2be69a8ae6d622a88b0f2b043d91d8d");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_depressed_ypack.ddt"), "bb9689bcfd6841fa5a584443c64b23fe");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_normal_ypack.ddt"), "363b4269fe27c5a703cd71d660787416");



                    }
                }

        }



        public static bool isJammsInstalled()
        {
            bool Result = false;
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");

                if (P != null)
                {
                    if (isMD5Equal(Path.Combine(P.ToString(), "Data", "uiarmybanners.xml"), "4aae1867a1919235613263c7fdbf3c9c") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnew.xml"), "0e74279f25b038d3b448b7a154509cd3") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnewmin.xml"), "fa44fa2a6d28a08a2ab221b2cdd05d05") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitycommandpanel.xml"), "3c633083941109ac38928faf47a7272f") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitydeck.xml"), "b793a30a83335126681796ed5a25aa7d") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitytransportpanelheader.xml"), "0199a270ede2dd8251398381cf153189") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uimainnew.xml"), "d0432c15c6b07fef7874cdfaf192221c") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnew.xml"), "fd0d9d461e9d2f5666fd938663e80692") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnewmin.xml"), "231027977041726355277849e4a947c4") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2min.xml"), "1549800a854aba6e64e066cca38abdf3") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiplayersummarydlg.xml"), "c1451fafd5a034f954d8069ea659c814") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2min.xml"), "ea49c3e7a124fbb1f743e23f6b9507b1") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontainsmin.xml"), "066124a3a763f78586df719a953e397b") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselection.xml"), "473a62100605e104d0ef780e3a254819") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselectionmin.xml"), "2e0a3b843a7d1c8bfc8a81b848350b3f") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3min.xml"), "3251ad20da75119e12061e177d8b3365") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanelmin.xml"), "ad3fc894688fadb9247b5f6433545e70") &&
                       isMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanelmin.xml"), "7268f64a1f18be9fce1f62d1beda7a53") &&

                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_active_ypack.ddt"), "bf93a0b74545b2bd1176b7cf381d542b") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_depressed_ypack.ddt"), "33f075fb02b9b706359074935b3f2240") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_normal_ypack.ddt"), "141f6f3943c712517748b775e94c9e9f") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_active_ypack.ddt"), "a3aa314cf0f3500f24476e0bf4015742") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_depressed_ypack.ddt"), "a295c0a43e1b9182dd45609033a6235a") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_normal_ypack.ddt"), "affebc408f675fefd29e54e649c85f33") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_active_ypack.ddt"), "c2be69a8ae6d622a88b0f2b043d91d8d") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_depressed_ypack.ddt"), "bb9689bcfd6841fa5a584443c64b23fe") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_normal_ypack.ddt"), "363b4269fe27c5a703cd71d660787416") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_ui_small_02.ddt"), "536ab0440638aac61d4679e9493e94ae") &&
                        isMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "Resource.ddt"), "926b480655d8700aa08128e331c64db6")

                        )

                        Result = true;
                }
            }
            return Result;
        }

        public static void RemoveJammsIfInstalled()
        {
            if (isJammsInstalled())
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiarmybanners.xml"), "4aae1867a1919235613263c7fdbf3c9c");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnew.xml"), "0e74279f25b038d3b448b7a154509cd3");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uicommandpanelnewmin.xml"), "fa44fa2a6d28a08a2ab221b2cdd05d05");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitycommandpanel.xml"), "3c633083941109ac38928faf47a7272f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitydeck.xml"), "b793a30a83335126681796ed5a25aa7d");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uihomecitytransportpanelheader.xml"), "0199a270ede2dd8251398381cf153189");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uimainnew.xml"), "d0432c15c6b07fef7874cdfaf192221c");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnew.xml"), "fd0d9d461e9d2f5666fd938663e80692");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiminimappanelnewmin.xml"), "231027977041726355277849e4a947c4");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uinativecommandpanel2min.xml"), "1549800a854aba6e64e066cca38abdf3");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiplayersummarydlg.xml"), "c1451fafd5a034f954d8069ea659c814");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitraderoutepanel2min.xml"), "ea49c3e7a124fbb1f743e23f6b9507b1");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uitrainsandcontainsmin.xml"), "066124a3a763f78586df719a953e397b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselection.xml"), "473a62100605e104d0ef780e3a254819");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitselectionmin.xml"), "2e0a3b843a7d1c8bfc8a81b848350b3f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunitstatpanel3min.xml"), "3251ad20da75119e12061e177d8b3365");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiunittrainingpanelmin.xml"), "ad3fc894688fadb9247b5f6433545e70");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "Data", "uiwonderpowerpanelmin.xml"), "7268f64a1f18be9fce1f62d1beda7a53");

                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_active_ypack.ddt"), "bf93a0b74545b2bd1176b7cf381d542b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_depressed_ypack.ddt"), "33f075fb02b9b706359074935b3f2240");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_all_normal_ypack.ddt"), "141f6f3943c712517748b775e94c9e9f");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_active_ypack.ddt"), "a3aa314cf0f3500f24476e0bf4015742");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_depressed_ypack.ddt"), "a295c0a43e1b9182dd45609033a6235a");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_econ_normal_ypack.ddt"), "affebc408f675fefd29e54e649c85f33");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_active_ypack.ddt"), "c2be69a8ae6d622a88b0f2b043d91d8d");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_depressed_ypack.ddt"), "bb9689bcfd6841fa5a584443c64b23fe");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_minimap_military_normal_ypack.ddt"), "363b4269fe27c5a703cd71d660787416");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "ingame_ui_small_02.ddt"), "536ab0440638aac61d4679e9493e94ae");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "ui", "ingame", "Resource.ddt"), "926b480655d8700aa08128e331c64db6");


                    }
                }
        }



        public static bool isArtyInstalled()
        {
            bool Result = false;
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");
                if (P != null)
                {
                    if (isMD5Equal(Path.Combine(P.ToString(), "art", "canon_circle.ddt"), "f6e0c75771d1c70c8add7444c969f906") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "circle_mis.ddt"), "1b03507d833eb6e651b2a416115c0e73") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "selection_circle.ddt"), "0ab76e4d2c290a45115b77dd781f40b3") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "cannon", "cannon.xml"), "06436871a816bb491d420d76d1a1452b") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "culverin", "culverin.ddt"), "915d4c640d215dfd36ea0fd0be3ea17b") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "culverin", "culverin.xml"), "a635825af9515b8438d318f58a931386") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "culverin", "culverin.xml.xmb"), "dab8644ee23a5ca42371524abae22d1e") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "falconet", "falconet.xml"), "e7dc5e9ffc1a8047dd0f66eaf79920e0") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "igc_great_bombard.xml"), "ca6b3f08a0593226862f798436c774de") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "igc_great_bombard.xml.xmb"), "a9fd305b31d59891b33961baf17563f5") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "Imperialgreat_bombard.xml"), "38ef0090d3a7a92b8d11eb0a3ef947b5") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "Imperialgreat_bombard.xml.xmb"), "22c55c46403e02fa377f6828c774a5de") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "horse_artillery", "horse_artillery.xml"), "32b66066fe74b0d808f8ce439e762e94") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "mortar", "mortar.xml"), "2c1f4244f7d2bdc213495db3be85c0b5") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "organ_gun", "organ_gun.xml"), "e6c930b688ff4d0bc709d563f6d4d8c1") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "rocket", "rocket.xml"), "c5beb07b20edd5e975d4dc8189d15696") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "great_bombard.xml"), "3f437b9c16f82e315e2a9cc4bccf259a") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "great_bombard.xml.xmb"), "c308af4196d7e1fa1f79c99fd565b3c1") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "Imperialgreat_bombard.xml"), "3f437b9c16f82e315e2a9cc4bccf259a") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "Imperialgreat_bombard.xml.xmb"), "c308af4196d7e1fa1f79c99fd565b3c1") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "consulate", "cannon", "cannon.xml"), "06436871a816bb491d420d76d1a1452b") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "consulate", "falconet", "falconet.xml"), "3c297f0d44557ba5e753f936929d9539") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "consulate", "light_cavalry", "light_cavalry_horse.xml"), "c2935ae1a7210c3bc3d0321bf7efa5f8") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "indians", "howdah", "howdah_elephant.xml"), "1c0b8173825f9aaa0cd7b7d9b85d3e25") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "indians", "siege_elephant", "siege_elephant.xml"), "a343890871173b8a841ad06454596134") &&
                      isMD5Equal(Path.Combine(P.ToString(), "art", "units", "priests", "spanish_priest.xml"), "5788313d7b899f1f896a7553a3b67d12")

                        )

                        Result = true;
                }


            }
            return Result;
        }

        public static void RemoveArtyIfInstalled()
        {
            if (isArtyInstalled())
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "canon_circle.ddt"), "f6e0c75771d1c70c8add7444c969f906");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "circle_mis.ddt"), "1b03507d833eb6e651b2a416115c0e73");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "selection_circle.ddt"), "0ab76e4d2c290a45115b77dd781f40b3");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "cannon", "cannon.xml"), "06436871a816bb491d420d76d1a1452b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "culverin", "culverin.ddt"), "915d4c640d215dfd36ea0fd0be3ea17b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "culverin", "culverin.xml"), "a635825af9515b8438d318f58a931386");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "culverin", "culverin.xml.xmb"), "dab8644ee23a5ca42371524abae22d1e");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "falconet", "falconet.xml"), "e7dc5e9ffc1a8047dd0f66eaf79920e0");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "igc_great_bombard.xml"), "ca6b3f08a0593226862f798436c774de");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "igc_great_bombard.xml.xmb"), "a9fd305b31d59891b33961baf17563f5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "Imperialgreat_bombard.xml"), "38ef0090d3a7a92b8d11eb0a3ef947b5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "great_bombard", "Imperialgreat_bombard.xml.xmb"), "22c55c46403e02fa377f6828c774a5de");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "horse_artillery", "horse_artillery.xml"), "32b66066fe74b0d808f8ce439e762e94");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "mortar", "mortar.xml"), "2c1f4244f7d2bdc213495db3be85c0b5");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "organ_gun", "organ_gun.xml"), "e6c930b688ff4d0bc709d563f6d4d8c1");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "rocket", "rocket.xml"), "c5beb07b20edd5e975d4dc8189d15696");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "great_bombard.xml"), "3f437b9c16f82e315e2a9cc4bccf259a");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "great_bombard.xml.xmb"), "c308af4196d7e1fa1f79c99fd565b3c1");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "Imperialgreat_bombard.xml"), "3f437b9c16f82e315e2a9cc4bccf259a");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "artillery", "unique_artillery", "Imperialgreat_bombard.xml.xmb"), "c308af4196d7e1fa1f79c99fd565b3c1");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "consulate", "cannon", "cannon.xml"), "06436871a816bb491d420d76d1a1452b");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "consulate", "falconet", "falconet.xml"), "3c297f0d44557ba5e753f936929d9539");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "consulate", "light_cavalry", "light_cavalry_horse.xml"), "c2935ae1a7210c3bc3d0321bf7efa5f8");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "indians", "howdah", "howdah_elephant.xml"), "1c0b8173825f9aaa0cd7b7d9b85d3e25");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "asians", "indians", "siege_elephant", "siege_elephant.xml"), "a343890871173b8a841ad06454596134");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "priests", "spanish_priest.xml"), "5788313d7b899f1f896a7553a3b67d12");
                    }


                }
        }

        public static bool isCowsInstalled()
        {
            bool Result = false;
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");
                if (P != null)
                {
                    if (isMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "cow", "cow.xml"), "ff7c95b16fe292991ab7cef5eb860f7e") &&
                   isMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "goat", "goat.xml"), "cf77e42a5c93fdcc90fbb5cc29528d83") &&
                   isMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "llama", "llama.xml"), "7c972ccaaa49f72f938a1902a3ab530e") &&
                   isMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "sheep", "sheep.xml"), "526d8df4a2adcd7b2af2aad4397ea04e") &&
                   isMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "water_buffalo", "water_buffalo.xml"), "3a3dd0d9f82d5c18e1d0ee652e2e1526") &&
                   isMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "yak", "yak.xml"), "13e979a3195c8c9284fce4727987c1a0")
                        )

                        Result = true;
                }
            }
            return Result;
        }

        public static void RemoveCowsIfInstalled()
        {
            if (isCowsInstalled())
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "cow", "cow.xml"), "ff7c95b16fe292991ab7cef5eb860f7e");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "goat", "goat.xml"), "cf77e42a5c93fdcc90fbb5cc29528d83");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "llama", "llama.xml"), "7c972ccaaa49f72f938a1902a3ab530e");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "sheep", "sheep.xml"), "526d8df4a2adcd7b2af2aad4397ea04e");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "water_buffalo", "water_buffalo.xml"), "3a3dd0d9f82d5c18e1d0ee652e2e1526");
                        DeleteMD5Equal(Path.Combine(P.ToString(), "art", "units", "animals", "yak", "yak.xml"), "13e979a3195c8c9284fce4727987c1a0");
                    }
                }
        }

        public static bool isKeysInstalled()
        {
            bool Result = false;
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");
                if (P != null)
                {
                    if (isMD5Equal(Path.Combine(P.ToString(), "data", "DefaultKeyMapY.xml"), "46af3938c1a76780a2123b9acaeea4a8"))
                        Result = true;
                }
            }
            return Result;
        }

        public static void RemoveKeysIfInstalled()
        {
            if (isKeysInstalled())
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        DeleteMD5Equal(Path.Combine(P.ToString(), "data", "DefaultKeyMapY.xml"), "46af3938c1a76780a2123b9acaeea4a8");

                    }
                }
        }


        public static string GetOptDirectoryTAD()
        {
            string Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Age of Empires 3", "Users3");
            Directory.CreateDirectory(Result);
            return Result;
        }

        public static string GetOptDirectoryTWC()
        {
            string Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Age of Empires 3", "Users2");
            Directory.CreateDirectory(Result);
            return Result;
        }

        public static string GetOptDirectoryNilla()
        {
            string Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Age of Empires 3", "Users");
            Directory.CreateDirectory(Result);
            return Result;
        }

        public static string GetHCDirectory()
        {
            string Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Age of Empires 3", "Savegame");
            Directory.CreateDirectory(Result);
            return Result;
        }

        public static void SetHC(string FilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            XmlElement Node = doc.DocumentElement;
            Node.SelectSingleNode("//Profile//UserInformation//LastSelectedSPHC").InnerText = "sp_Amsterdam_homecity.xml";
            doc.Save(FilePath);
        }

        public static bool IsFontInstalled(string Name)
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");
                if (P != null)
                {
                    if (File.Exists(Path.Combine(P.ToString(), "FONTS", "fonts2.xml")) && File.Exists(Path.Combine(P.ToString(), "FONTS", "fonts3.xml")))
                    {
                        string s1 = File.ReadAllText(Path.Combine(P.ToString(), "FONTS", "fonts2.xml"));
                        string s2 = File.ReadAllText(Path.Combine(P.ToString(), "FONTS", "fonts3.xml"));
                        return s1.IndexOf("\"" + Name + "\"") > -1 && s2.IndexOf("\"" + Name + "\"") > -1;
                    }
                }
            }
            return false;
        }

        public static void ChangeFont(string Name)
        {
            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = AS.GetValue("setuppath");
                if (P != null)
                {
                    if (File.Exists(Path.Combine(P.ToString(), "FONTS", "fonts2.xml")) && File.Exists(Path.Combine(P.ToString(), "FONTS", "fonts3.xml")))
                    {
                        string s1 = File.ReadAllText(Path.Combine(P.ToString(), "FONTS", "fonts2.xml"));
                        string s2 = File.ReadAllText(Path.Combine(P.ToString(), "FONTS", "fonts3.xml"));
                        s1 = s1.Replace("\"Arial\"", "\"" + Name + "\"");
                        s1 = s1.Replace("\"Formal436 BT\"", "\"" + Name + "\"");
                        //    s1 = s1.Replace("UTF-16", "UTF-8");
                        s2 = s2.Replace("\"Arial\"", "\"" + Name + "\"");
                        s2 = s2.Replace("\"Formal436 BT\"", "\"" + Name + "\"");
                        File.WriteAllText(Path.Combine(P.ToString(), "FONTS", "fonts2.xml"), s1, Encoding.UTF8);
                        File.WriteAllText(Path.Combine(P.ToString(), "FONTS", "fonts3.xml"), s2, Encoding.UTF8);
                    }
                }
            }
        }


        public static void SetOptions(string FilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            XmlElement Node = doc.DocumentElement;
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionsoundlevel']").InnerText = "1";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionmusiclevel']").InnerText = "1";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optiongrfxshaderquality']").InnerText = "0";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optiongrfxres']").InnerText = SystemParameters.PrimaryScreenWidth.ToString() + " x " + SystemParameters.PrimaryScreenHeight.ToString();
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optiongrfxwindowmode']").InnerText = "true";
           
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optioncamerarotation']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optioneasydragmilitary']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionuishowtraining']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionuishowgametime']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionuishowvillagertasks']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionuishowscore']").InnerText = "true";

            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optioncamerazoom']").InnerText = "3";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionambientsoundlevel']").InnerText = "1";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionadvancedformationui']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionskirmishnickname']").InnerText = "User";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionenablerobustrollover']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionminimizedui']").InnerText = "true";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionminimizedchatui']").InnerText = "true";
            if (Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionshowhponalt']") != null)
                Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionshowhponalt']").InnerText = "true";
            if (Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionadvancedunittypeinfo']") != null)
                Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionadvancedunittypeinfo']").InnerText = "true";
            if (Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionadvancedformationui']") != null)
                Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionadvancedformationui']").InnerText = "true";


            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionlanguagefilter']").InnerText = "false";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionsoundchatvolume']").InnerText = "1";
            Node.SelectSingleNode("//GameSettings[@Name='GameOptions']//Settings//Setting[@Name='optionmusiconoff']").InnerText = "false";

            Node.SelectSingleNode("//Profile//UserInformation//SaveESOName").InnerText = "1";
            if (Node.SelectSingleNode("//Profile//UserInformation//ShowESOWarning") != null)
                Node.SelectSingleNode("//Profile//UserInformation//ShowESOWarning").InnerText = "0";

            doc.Save(FilePath);
        }
    }
}
