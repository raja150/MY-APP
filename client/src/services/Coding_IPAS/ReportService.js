import APIService from '../apiservice';

class ReportService {
    async CD_DayPerformance() {
        return await APIService.getAsync(`Coding_IPAS/Report/CD_DayPerformance`)
    }
    async CD_MonthPerformance() {
        return await APIService.getAsync(`Coding_IPAS/Report/CD_MonthPerformace`)
    }
    async CD_PerformnacePayCut() {
        return await APIService.getAsync(`Coding_IPAS/Report/CD_PerformancePayCut`)
    }
}
export default new ReportService();