import APIService from './apiservice';
class OrganizationService{
    async getOrganizationByPatient(patient){
        return await APIService.getAsync(`Setup/Organization/${patient}`);
    }

    async getOrganisationList(){
      return  await APIService.getAsync('Setup/Organization');
    }
    async CheckMailExist(data){
      return await APIService.getAsync(`Auth/MailCheck/${data.mailCheck}`);
    }
}
export default new OrganizationService();