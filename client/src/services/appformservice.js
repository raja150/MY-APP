import APIService from './apiservice';

class AppFormsService {

    get(formid, callback) {

        APIService.get(
            "/appforms/" + formid,
          
            (status, data) =>  callback(status, data)
        )
    }
    async postLookupValues(code ,text){
        return await APIService.postAsync('LookupValues', { code , text})
    }
    async getLookupValuesByObservarion(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getLookupValuesBySymptoms(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getLookupValuesByDiagnosis(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getLookupValuesByTreatmentnote(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getLookupValuesByTestName(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getLookupValuesByLab(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getLookupValuesByUsageType(lookupCodes){
        return await APIService.getAsync('LookupValues/' + lookupCodes)
    }
    async getAppForms(id){
      return  await APIService.getAsync('appforms/' + id)
    }

    async getAppFormsById(module,tabJson,editEntityId){
      return  await APIService.getAsync(`${module}/${tabJson}/${editEntityId}`)
    }
    async putAppFormById(module,tabJson,values){
       return await APIService.putAsync(`${module}/${tabJson}`, values);
    }
    async postAppForm(module,tabJson,values){
      return  await APIService.putAsync(`${module}/${tabJson}`, values);
    }

    async appformsPaginate(module,entityName,qstring){
      return  await APIService.getAsync(`${module}/${entityName}/Paginate?${qstring}`)
    }
};


export default new AppFormsService();
