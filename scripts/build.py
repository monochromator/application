
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

UI_PATH = "./ui/"


# Removing build
if os.path.isdir(APP_OUTPUT_PATH):
    shutil.rmtree(APP_OUTPUT_PATH)


def extractCef(sup_os: str):
    if sup_os == "linux-x64" or sup_os == "win-x64":
        # Copy release
        os.system("tar -xvf " + APP_OUTPUT_PATH + sup_os +
                  "/cef.tar.bz2 -C " + APP_OUTPUT_PATH + sup_os + " --wildcards 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_" + SUPPORTED_OS[sup_os]["cfPrefix"] + "_minimal/Release/*' --strip 2")
        # Copy resources
        os.system("tar -xvf " + APP_OUTPUT_PATH + sup_os +
                  "/cef.tar.bz2 -C " + APP_OUTPUT_PATH + sup_os + " --wildcards 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_" + SUPPORTED_OS[sup_os]["cfPrefix"] + "_minimal/Resources/*' --strip 2")

    elif sup_os == "osx-x64":
        # Copy release
        os.system("tar -xvf " + APP_OUTPUT_PATH + sup_os +
                  "/cef.tar.bz2 -C " + APP_OUTPUT_PATH + sup_os + " 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_macosx64_minimal/Release/Chromium Embedded Framework.framework/Chromium Embedded Framework' --strip 3")

        # Rename that file to libcef.dylib
        os.rename(APP_OUTPUT_PATH + sup_os + "/Chromium Embedded Framework",
                  APP_OUTPUT_PATH + sup_os + "/libcef.dylib")

        # Copy Libraries
        os.system("tar -xvf " + APP_OUTPUT_PATH + sup_os +
                  "/cef.tar.bz2 -C " + APP_OUTPUT_PATH + sup_os + " --wildcards 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_macosx64_minimal/Release/Chromium Embedded Framework.framework/Libraries/*' --strip 4")
        # Copy Resources
        os.system("tar -xvf " + APP_OUTPUT_PATH + sup_os +
                  "/cef.tar.bz2 -C " + APP_OUTPUT_PATH + sup_os + " --wildcards 'cef_binary_83.5.0+gbf03589+chromium-83.0.4103.106_macosx64_minimal/Release/Chromium Embedded Framework.framework/Resources/*' --strip 4")
        pass
    else:
        raise NameError("OS not found")


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

        extractCef(sup_os)

        # Removing tar
        os.system("rm " + buildPath + "/cef.tar.bz2")


def buildUi():
    os.system("npm install --prefix " + UI_PATH)
    os.system("npm run --prefix " + UI_PATH + " build")

    # Move dashboard to each os
    for sup_os in SUPPORTED_OS:
        buildPath = APP_OUTPUT_PATH + sup_os
        os.system("cp -r " + UI_PATH + "dist " + buildPath + "/ui")


# Build app
buildApp()

# Build ui
buildUi()
