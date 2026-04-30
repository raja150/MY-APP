import APIService from 'services/apiservice'
const apiUrl='PayRoll/FinancialYear'

class FinancialYear{
    async getList(){
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
}
export default new FinancialYear()