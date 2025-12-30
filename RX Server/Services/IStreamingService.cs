namespace RX_Server.Services
{
    public interface IStreamingService
    {
        //Lay duong dan stream cho bai hat dua tren goi dang ki cua user
        Task<string> GetAudioFilePathAsync(int songId, int userSubscriptionId);

        //Xu ly upload bai hat moi: Luu file goc va convert sang cac chat luong khac. Return duration (seconds).
        Task<double> ProcessUploadedFileAsync(IFormFile file, int songId);

        //Xoa file nhac khi bai hat bi xoa
        Task DeleteSongFilesAsync(int songId);
    }
}