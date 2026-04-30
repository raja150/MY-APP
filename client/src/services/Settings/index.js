import APIService from '../apiservice';

class SettingsService{
    async getSequenceNo(){
      return  await APIService.getAsync('SequenceNo');
    }

    async putSequenceNo(items){
     return   await APIService.putAsync('SequenceNo', items)
    }

    async getRolePages(){
       return await APIService.getAsync('Role/GetPages')
    }
    async getRolePagesById(roleId){
      return  await APIService.getAsync('Role/' + roleId)
    }
    async updateAsync(data) {
        if (data.id) {
            await APIService.putAsync('Role', data);
        } else {
            await APIService.postAsync('Role', data);
        }
    }
    
    async postLoginBox(data) {
        return await APIService.postAsync('Auth', data)
    }

    async getRoleList(){
       return await APIService.getAsync('Role')
    }
    async getPages(){
        return await APIService.getAsync('Role/Pages')
     }
     async getReports(){
        return await APIService.getAsync('Role/Reports')
     }
}
export default new SettingsService();