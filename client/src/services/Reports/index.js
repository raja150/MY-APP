import APIService from '../apiservice';

class ReportService {
    async employeeProfile() {
        return await APIService.getAsync(`Emp_Report/EmployeeProfile`)
    }
    
    async getReportData(path) {
        
        return await APIService.getAsync(`/${path}`)
    }
   
    async getModules(){
        return await APIService.getAsync(`Role/Modules`)
    }
    async getReport(groupId){
        return await APIService.getAsync(`Role/GetReport/${groupId}`)
    }
}
export default new ReportService();