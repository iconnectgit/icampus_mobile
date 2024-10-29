using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Forms.PopupForms;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class CropPhotoPopup : Popup
{
    public CropPhotoPopup()
    {
        InitializeComponent();
    }

    private double _originalX, _originalY;
    private double startScale, currentScale;

    private double originalWidth;
    private double startX;
    private double originalHeight;
    private double startY;
    private double initialX;
    private double initialY;
    private bool isResizing;

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var viewModel = BindingContext as CropPhotoPopupForm;
        if (e.StatusType == GestureStatus.Started)
        {
            // Store the initial width, height, touch position, and initial position of the crop box when the pan gesture starts
            originalWidth = cropBox.Width;
            originalHeight = cropBox.Height;
            startX = e.TotalX;
            startY = e.TotalY;
            initialX = cropBox.TranslationX;
            initialY = cropBox.TranslationY;

            // Determine if the user is near the edges to start resizing
            isResizing = IsNearEdge(e.TotalX, e.TotalY);
        }
        else if (e.StatusType == GestureStatus.Running)
        {
            var deltaX = e.TotalX - startX;
            var deltaY = e.TotalY - startY;
            var newWidth = originalWidth;
            var newHeight = originalHeight;

            if (isResizing)
            {
                // Handle resizing based on movement direction
                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                {
                    newWidth = Math.Max(originalWidth + deltaX, 50);
                    newWidth = Math.Min(newWidth,
                        imageToCrop.Width - (cropBox.X + cropBox.TranslationX)); // Clamp to image width
                    cropBox.WidthRequest = newWidth;
                }
                else
                {
                    newHeight = Math.Max(originalHeight + deltaY, 50);
                    newHeight = Math.Min(newHeight,
                        imageToCrop.Height - (cropBox.Y + cropBox.TranslationY)); // Clamp to image height
                    cropBox.HeightRequest = newHeight;
                }

                // Update the ViewModel with the new width and height
                viewModel.CropBoxWidth = newWidth;
                viewModel.CropBoxHeight = newHeight;
            }
            else
            {
                // Move the entire crop box, ensuring it stays within the bounds of the image
                var newTranslationX = Math.Max(-cropBox.X,
                    Math.Min(initialX + deltaX, imageToCrop.Width - cropBox.Width - cropBox.X));
                var newTranslationY = Math.Max(-cropBox.Y,
                    Math.Min(initialY + deltaY, imageToCrop.Height - cropBox.Height - cropBox.Y));

                cropBox.TranslationX = newTranslationX;
                cropBox.TranslationY = newTranslationY;

                // Update the ViewModel with the new X and Y positions
                viewModel.CropBoxX = cropBox.X + newTranslationX;
                viewModel.CropBoxY = cropBox.Y + newTranslationY;
            }
        }
    }

    // Utility method to determine if the user is near the edge of the crop box
    private bool IsNearEdge(double touchX, double touchY)
    {
        double edgeThreshold = 20; // Allow 20 pixels near the edge for resizing
        var leftEdge = cropBox.X;
        var rightEdge = cropBox.X + cropBox.Width;
        var topEdge = cropBox.Y;
        var bottomEdge = cropBox.Y + cropBox.Height;

        return Math.Abs(touchX - leftEdge) <= edgeThreshold ||
               Math.Abs(touchX - rightEdge) <= edgeThreshold ||
               Math.Abs(touchY - topEdge) <= edgeThreshold ||
               Math.Abs(touchY - bottomEdge) <= edgeThreshold;
    }

    private void MenuClosedClick(object? sender, EventArgs eventArgs)
    {
        Close();
    }
}