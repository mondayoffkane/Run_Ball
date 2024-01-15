#include "Unity/IUnityGraphics.h"
#import "UnityAppController.h"

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(IUnityInterfaces* unityInterfaces);
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload();

@interface AdvertyAppController : UnityAppController
{
}
- (void)shouldAttachRenderDelegate;
@end
@implementation AdvertyAppController

- (void)shouldAttachRenderDelegate
{
    //NOTE: unlike desktops where plugin dynamic library is automatically loaded and registered
    // we need to do that manually on iOS. If you already have own custom app controller, move
    // shouldAttachRenderDelegate method to your implementation and be free to delete this file from project.
    UnityRegisterRenderingPluginV5(&UnityPluginLoad, &UnityPluginUnload);
}

@end
IMPL_APP_CONTROLLER_SUBCLASS(AdvertyAppController);
