namespace ReportService.Domain
{
    public enum EPayrollStatus
    {
        //Черновик
        Draft,
        //На подписи
        InSigning,
        //В обработке
        SendToBank,
        //Исполнен
        Executed,
        //Отказан
        Rejected,
        //Частично исполнен
        PartiallyExecuted
    }
}