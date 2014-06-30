using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AnimatWizard
{
    class Program
    {
        static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("AnimatProjGen [-b] [-v] [-r] -name ProjName");
            Console.WriteLine("-b: Optional include bullet references");
            Console.WriteLine("-v: Optional include vortex references");
            Console.WriteLine("-r: Optional include robotics references");
            Console.WriteLine("-name ProjName: Name of the new GUI and sim projects. GUI and Sim will be appended to them.");
        }


        static void Main(string[] args)
        {
            bool bBulletIncludes = false;
            bool bVortexIncludes = false;
            bool bRoboticsIncludes = false;
            bool bNextName = false;
            string strProjName = "";
            string strSDKRoot = "C:\\Projects\\AnimatLabSDK\\AnimatLabPublicSource";

            foreach (string strArg in args)
            {
                if (strArg.Trim().ToLower() == "-b")
                    bBulletIncludes = true;
                else if (strArg.Trim().ToLower() == "-v")
                    bVortexIncludes = true;
                else if (strArg.Trim().ToLower() == "-r")
                    bRoboticsIncludes = true;
                else if (strArg.Trim().ToLower() == "-name")
                    bNextName = true;
                else if (bNextName == true)
                    strProjName = strArg;
            }

            if (strProjName.Trim().Length == 0)
            {
                Console.WriteLine("Project name is blank. ");
                PrintHelp();
                return;
            }
            
            //First create new GUI and Sim folders
            string strGuiPath = strSDKRoot + "\\Libraries\\" + strProjName + "GUI";
            string strGuiVsPath = strGuiPath + "\\Projects_VisualStudio";
            string strGuiMonoPath = strGuiPath + "\\Projects_Mono";
            string strGuiCbPath = strGuiPath + "\\Projects_CodeBlocks";
            Directory.CreateDirectory(strGuiPath);
            Directory.CreateDirectory(strGuiVsPath);
            Directory.CreateDirectory(strGuiMonoPath);
            Directory.CreateDirectory(strGuiCbPath);

            string strSimPath = strSDKRoot + "\\Libraries\\" + strProjName + "Sim";
            string strSimVsPath = strSimPath + "\\Projects_VisualStudio";
            string strSimMonoPath = strSimPath + "\\Projects_Mono";
            string strSimCbPath = strSimPath + "\\Projects_CodeBlocks";
            Directory.CreateDirectory(strSimPath);
            Directory.CreateDirectory(strSimVsPath);
            Directory.CreateDirectory(strSimMonoPath);
            Directory.CreateDirectory(strSimCbPath);


        }
    }
}
