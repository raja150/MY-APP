import APIService from '../apiservice'
const apiUrl = 'PayRoll/Bank'
class BankService {
    async getList(){
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
}
export default new BankService()