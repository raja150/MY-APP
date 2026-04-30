import APIService from '../apiservice'
const apiUrl = 'Helpdesk/HelpTopicSub'

class SubTopicService{
    async SubTopicByHelp(helpTopicId){
        return await APIService.getAsync(`${apiUrl}/SubTopics/${helpTopicId}`)
    }
}

export default new SubTopicService();