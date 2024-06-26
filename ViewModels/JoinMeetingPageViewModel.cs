﻿namespace DemyAI.ViewModels;

public partial class JoinMeetingPageViewModel(
    IAudioManager audioManager,
    IMeetingService meetingService, IAppService appService,
    IDataService<DemyUser> dataService) : BaseViewModel {

    public ObservableCollection<Datum> meetingData { get; set; } = [];

    bool isExpanded;

    [ObservableProperty]
    bool isWebViewVisible;

    [ObservableProperty]
    string? roomName;

    [ObservableProperty]
    bool isMeetingToolbarVisible;

    [ObservableProperty]
    bool isSearhBarVisible = true;

    [ObservableProperty]
    bool isTakeBreakTime;

    [ObservableProperty]
    string webviewSorce;

    [ObservableProperty]
    string? elapsedTimeString;

    private readonly Stopwatch? stopwatch = new();

    private Timer? meetingTimer;
    private Timer? breakTimeTimer;
    private Timer? CheckPresenceTimer;

    private bool meetingInProgress = false;

    [RelayCommand]
    async Task EntryCompleted() {
        await VerifyAddressAsync(appService);
        IsSearhBarVisible = false;
        IsMeetingToolbarVisible = true;
        WebviewSorce = $"{Constants.BASETTING_URL}{RoomName}";
    }

    [RelayCommand]
    void ShowParticipant(object o) {

        var participantList = o as CollectionView;

        if (participantList != null) {
            if (isExpanded == false) {
                ExpandAnim(participantList);
            } else {
                CollapeAnim(participantList);
            }
        }
    }

    private void ExpandAnim(CollectionView? participantList) {
        var anim = new Animation((current) => {

            participantList!.WidthRequest = current;
        }, 0, 300, null);

        anim.Commit(participantList, "Expand", finished: (value, canceled) => {

            participantList!.IsVisible = true;
            isExpanded = true;
        });
    }

    private void CollapeAnim(CollectionView? participantList) {
        var anim = new Animation((current) => {

            participantList!.WidthRequest = current;
        }, 300, 0, null);

        anim.Commit(participantList, "collapse", finished: (value, canceled) => {

            participantList!.IsVisible = false;
            isExpanded = false;
        });
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
        if (!string.IsNullOrEmpty(RoomName)) {
            GotoSite();
        } else {
            await appService.DisplayAlert("Error", "Please paste the meeting name",
                "OK");
        }
    }
    private void GotoSite() {
        IsWebViewVisible = true;
        if (IsWebViewVisible) {
            IsMeetingToolbarVisible = true;
            CheckPresenceTimer = new Timer(
           async (state) => await UpdateMeetingData(),
           null,
           TimeSpan.FromSeconds(15),
           TimeSpan.FromSeconds(15));
        }
    }

    public async Task UpdateMeetingData() {

        var meetingFromService = await meetingService.GetMeetingPresence(
            Constants.DAILY_AUTH_TOKEN, RoomName!);

        MainThread.BeginInvokeOnMainThread(() => {

            meetingData.Clear();

            foreach (var item in meetingData.ToList()) {

                if (!meetingFromService.data.Contains(item)) {

                    meetingData.Remove(item);
                }
            }

            foreach (var item in meetingFromService.data) {
                if (!meetingData.Contains(item)) {
                    meetingData.Add(item);
                }
            }

            if (meetingData.Count > 0) {
                meetingInProgress = true;
            }

            if (meetingInProgress) {
                StartBreakTimeTimer();
                StartMeetingChronometer();
                meetingInProgress = false;
            }
        });


        await dataService.UpdateAsync(Constants.MEETINGS, RoomName!,
            meetingFromService);
    }
}

