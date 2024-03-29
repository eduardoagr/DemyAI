﻿namespace DemyAI.ViewModels;

public partial class JoinMeetingPageViewModel(AppShell appShell, IHttpService httpService, IAudioManager audioManager,
    IMeetingService meetingService, IAppService appService, IDataService<DemyUser> dataService) : BaseViewModel {

    double currentFlyoutWith = appShell.FlyoutWidth;

    public ObservableCollection<string> names { get; set; } = [];


    [ObservableProperty]
    bool meetingSearchVibiility = true;

    [ObservableProperty]
    bool toolbarVisibility;

    [ObservableProperty]
    bool isWebViewVisible;

    [ObservableProperty]
    string? roomUrl;

    [ObservableProperty]
    string? roomName;

    [ObservableProperty]
    bool isTakeBreakTime;

    [ObservableProperty]
    string? elapsedTimeString;

    private readonly Stopwatch? stopwatch = new();

    private Timer? meetingTimer;
    private Timer? breakTimeTimer;

    Datum? meetingData;

    [RelayCommand]
    async Task EntryCompleted() {
        await VerifyAddressAsync(appService);
    }
    private void StartBreakTimeTimer() {
        breakTimeTimer = new Timer(
            state => ShowAlert(new BreakTimePopUp(audioManager)),
            null,
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(30));
    }
    private void StartMeetingChronometer() {
        stopwatch!.Start();
        meetingTimer = new Timer(state => UpdateElapsedTime(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromMicroseconds(1005.0));
    }
    private void UpdateElapsedTime() {
        TimeSpan elapsed = stopwatch!.Elapsed;
        ElapsedTimeString = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

    }
    private async Task VerifyAddressAsync(IAppService appService) {
        if(RoomUrl is not null && RoomUrl!.Contains("demy-ia")) {
            GotoSite();
        } else {
            await appService.DisplayAlert("Error", "Please paste a valid meeting link", "OK");
        }
    }
    private void GotoSite() {
        if(!string.IsNullOrEmpty(RoomUrl)) {
            RoomName = RoomUrl.Split('/').LastOrDefault();
            IsWebViewVisible = true;
            if(IsWebViewVisible) {
                ToolbarVisibility = !ToolbarVisibility;
                MeetingSearchVibiility = !MeetingSearchVibiility;
                appShell.FlyoutWidth = 0;
                StartBreakTimeTimer();
                StartMeetingChronometer();
            }
        }
    }
    public async Task UpdateMeetingData() {

        var meetingFromService = await meetingService.GetMeetingPresence(
            Constants.DAILY_AUTH_TOKEN);

        meetingData = meetingFromService.data.FirstOrDefault()!;

        await dataService.UpdateAsync("Meetings", RoomName!, meetingData);
    }
}

