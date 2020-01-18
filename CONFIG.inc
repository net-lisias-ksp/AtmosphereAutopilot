source ../CONFIG.inc

PACKAGE=AtmosphereAutopilot
TARGETDIR="$PACKAGE"
TARGETBINDIR="$TARGETDIR/Plugins"
PROJECTSDIR="Sources/"
DLLS="AtmosphereAutopilot AtmosphereAutopilot.UI"

VERSION=$( cat $PACKAGE.version | tr '\n' ' ' | sed -n -E 's/^.*?"VERSION\":\{.*"MAJOR":([0-9]+?),.*?"MINOR":([0-9]+?),.*?"PATCH":([0-9]+?),.*?"BUILD":([0-9]+?).*?\}.*$/\1.\2.\3.\4/p' )
