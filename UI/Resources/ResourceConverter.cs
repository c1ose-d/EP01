namespace UI.Resources {
    internal static class ResourceConverter {
        public static BitmapImage ToBitmapImage(byte[] bytes) {
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}