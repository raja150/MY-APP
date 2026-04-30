import APIService from '../apiservice'
const apiUrl = 'PayRoll/PayMonth'
const apiUrl1 = 'SelfService/AnnualSalary'

class PayMonthService {
    async ReleasePaySheet(id) {
        return await APIService.getAsync(`${apiUrl}/ReleaseSalary/${id}`)
    }
    async Taxes(id) {
        return await APIService.getAsync(`${apiUrl}/Taxes/${id}`)
    }
    async PaySheetDownload(id, pwd) {
        return await APIService.getBlobAsync(`${apiUrl}/PaySheet/${id}/${pwd}`)
    }
    async getHoldSalary(id) {
        return await APIService.getAsync(`${apiUrl}/HoldSalary/${id}`)
    }
    async getPayMonthEmpS(query) {
        return await APIService.getAsync(`${apiUrl}/Employees?${query}`);
    }
    async getHoldSalEmps(query) {
        return await APIService.getAsync(`${apiUrl}/HoldEmployees?${query}`)
    } 
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async SendPaySlips(id) {
        return await APIService.getAsync(`${apiUrl}/SendPayslips?PayMonthID=${id}`)
    }
    async EmpPaySlip(empId, monthId) {
        return await APIService.getAsync(`${apiUrl}/PaySlip?id=${monthId}&employeeId=${empId}`)
    }
    async MyPaySlip(monthId) {
        return await APIService.getAsync(`${apiUrl}/MyPaySlip?id=${monthId}`)
    }
    async Months() {
        return await APIService.getAsync(`${apiUrl}/Months`)
    }
    async MyPayMonths(id) {
        return await APIService.getAsync(`${apiUrl}/MyPayMonths`)
    }
    async DownloadPaySlip(id, monthId) {
        return await APIService.getBlobAsync(`${apiUrl}/DownloadPaySlip/${monthId}/${id}`)
    }
    async DownloadMyPaySlip(monthId) {
        return await APIService.getBlobAsync(`${apiUrl}/DownloadMyPaySlip/${monthId}`)
    }
    async getAnnualSalaryInfo(fiYrId) {
        return await APIService.getAsync(`${apiUrl}/AnnualSalary/${fiYrId}`)
    } 
    async HasFormSixteen(financialYearId) {
        return await APIService.getBlobAsync(`${apiUrl1}/HasFormSixteen`)
    }
    async DownloadMyFormSixteen(financialYearId) {
        return await APIService.getBlobAsync(`${apiUrl1}/FormSixteen?id=${financialYearId}`)
    }
    async DownloadMyFormSixteenB(financialYearId) {
        return await APIService.getBlobAsync(`${apiUrl1}/FormSixteenB?id=${financialYearId}`)
    }
    async BankSalaryDetails(payMonthId) {
        return await APIService.getAsync(`${apiUrl}/BankSalaryDetails?payMonthId=${payMonthId}`)
    } 
}
export default new PayMonthService