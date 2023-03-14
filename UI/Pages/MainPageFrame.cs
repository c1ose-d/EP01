namespace UI.Pages {
    internal class MainPageFrame : Frame {
        private static MainPageFrame? Instance { get; set; }

        protected MainPageFrame() { }

        public static MainPageFrame? GetInstance() {
            Instance ??= new MainPageFrame();
            return Instance;
        }
    }
}