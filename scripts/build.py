
import os
import shutil

# Monochromator build script
APP_PATH = "./app"
APP_OUTPUT_PATH = "./build/"
SUPPORTED_OS = {"linux-x64":
                {
                    "cfPrefix": "linux64"
                },
                "win-x64": {
                    "cfPrefix": "windows64"
                },
                "osx-x64": {
                    "cfPrefix": "macosx64"
                }
                }

CF_DOWNLOAD_LINK_1 = "https://cef-builds.spotifycdn.com/"
CF_DOWNLOAD_LINK_2 = "cef_binary_83.5.0%2Bgbf03589%2Bchromium-83.0.4103.106_"
CF_DOWNLOAD_LINK_3 = "_minimal.tar.bz2"

# Removing build
if APP_OUTPUT_PATH in os.listdir("."):
    shutil.rmtree(APP_OUTPUT_PATH)


def extractCef(sup_os: str):
    if str(sup_os) == "linux-x64":
        print("ex comm : " + "tar -xvf " + APP_OUTPUT_PATH + sup_os +
              "/cef.tar.bz2 --wildcards 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_linux64_minimal/Release/*' --strip 2 -C " + APP_OUTPUT_PATH + sup_os)
        os.system("tar -xvf " + APP_OUTPUT_PATH + sup_os +
                  "/cef.tar.bz2 --wildcards 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_linux64_minimal/Release/*' --strip 2 -C " + APP_OUTPUT_PATH + sup_os)

        # -C KO
        pass
    elif sup_os == "win-x64":
        pass
    elif sup_os == "osx-x64":
        pass
    else:
        print(sup_os)
        print(str(sup_os))
        print(str(sup_os) == "linux-x64")
        raise NameError("Not found")


def buildApp():

    # Build for each os
    for sup_os in SUPPORTED_OS:
        # App build
        print("Building app for : " + sup_os)
        buildPath = APP_OUTPUT_PATH + sup_os
        os.system("dotnet publish " + APP_PATH +
                  " -c Release -o " + buildPath + " -r " + sup_os)

        # CF download
        dlLink = CF_DOWNLOAD_LINK_1 + CF_DOWNLOAD_LINK_2 + \
            SUPPORTED_OS[sup_os]["cfPrefix"] + CF_DOWNLOAD_LINK_3
        print("download link : " + dlLink)

        os.system("wget -O " + buildPath + "/cef.tar.bz2 " + dlLink)

        print("Wget ok")
        extractCef(sup_os)


buildApp()
