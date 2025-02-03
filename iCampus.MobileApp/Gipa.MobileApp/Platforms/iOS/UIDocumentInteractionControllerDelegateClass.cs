using UIKit;

namespace Gipa.MobileApp;

public class UIDocumentInteractionControllerDelegateClass : UIDocumentInteractionControllerDelegate
{
    UIViewController ownerVC;

    public UIDocumentInteractionControllerDelegateClass(UIViewController ownerVC)
    {
        this.ownerVC = ownerVC;
    }

    public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
    {
        return ownerVC;
    }

    public override UIView ViewForPreview(UIDocumentInteractionController controller)
    {
        return ownerVC.View;
    }
}