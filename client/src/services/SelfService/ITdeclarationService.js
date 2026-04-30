import APIService from '../apiservice';

class ITDeclarationService {
    async postDeclaration(data) {
        if (data.id) {
            return await APIService.putAsync(`PayRoll/Declaration`, data)
        } else {
            return await APIService.postAsync(`PayRoll/Declaration`, data)

        }
    }
  
}
export default new ITDeclarationService()