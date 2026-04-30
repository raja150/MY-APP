import APIService from '../apiservice';
const apiUrl = 'PayRoll/Salary'
class SalaryService {
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getSalaryInfo(id) {
        return await APIService.getAsync(`PayRoll/Salary/${id}`)
    }
    async getTemplates() {
        return await APIService.getAsync(`Payroll/Template/GetList`)
    }
    async getTemplateComponentsById(templateId) {
        return await APIService.getAsync(`PayRoll/Salary/Components/${templateId}`)
    }
    async getEarningComponents() {
        return await APIService.getAsync(`Payroll/EarningComponent/GetList`)
    }
    async getDeductionComponents() {
        return await APIService.getAsync(`Payroll/DeductionComponent/GetDeductions`)
    }
    async searchEmp(query) {
        return await APIService.getAsync(`Organization/Employee/searchEmp?name=${query}`)
    }
    async updateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`PayRoll/Salary`, data)
        }
        return await APIService.postAsync(`PayRoll/Salary`, data)
    }
    async getSalary(EmpId) {
        return await APIService.getAsync(`PayRoll/Salary/Salary/${EmpId}`);
    }
    async EmployeeSalaryDownload(password) {
        return await APIService.getBlobAsync(`${apiUrl}/EmployeeSalary?password=${password}`)
    }
}
export default new SalaryService();