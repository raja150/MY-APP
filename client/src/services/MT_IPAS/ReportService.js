import APIService from '../apiservice';

class ReportService {
    async MT_PerformancePayCut() {
        return await APIService.getAsync(`MT_IPAS/MTIPASReport/MT_PerformancePayCut`)
    }
}
export default new ReportService();