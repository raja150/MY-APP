import queryString from 'query-string';
import Tabs, { TabPane } from 'rc-tabs';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import TabContent from 'rc-tabs/lib/TabContent';
import { default as React, Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Loader from 'react-loaders';
import APIService from '../../../services/apiservice';
import * as crypto from '../../../utils/Crypto';
import AxListSecondary from './AxListSecondary';
import AxWizard from './AxWizard';
import * as compare from '../../../utils/Compare';

export default function AxPage(props) {
  const [entityData, setEntityDataValue] = useState({ isLoading: true });
  const [refEntityId, setRefEntityId] = useState(null);
  //Get query string values
  const parsed = queryString.parse(props.location.search);

  const fid = parsed.q;
  const eid = parsed.r ? crypto.decrypt(parsed.r) : (props.r ? props.r : null);

  useEffect(() => {
    const fetchData = async () => {
      await APIService.getAsync('appForms/' + fid).then((response) => {
        const json = JSON.parse(response.data.json);
        setRefEntityId(eid ? eid : '');
        setEntityDataValue({
          isLoading: false, formId: fid, entityName: response.data.name,
          module: json.module, entityJSON: json
        })
      });
    };
    fetchData();
  }, []);

  //This called from when primary tab wizard is saved. Parameter Id is API return value
  //To update the entity id (Note: for primary tab reference entity id and entity id both are same)
  const handleFromSaved = id => {
    if (compare.isEqual(entityData.entityJSON.tabs.length, 1)) {
      props.history.push('/d/list/' + fid);
    } else {
      setRefEntityId(id ? id : '');
    }
  };

  return (
    <Fragment>
      {entityData.isLoading ?
        <Loader type="ball-grid-pulse" />
        : (
          //When reference entity is primary tab entity id 
          //when it is not available redirect to primary tab wizard screen
          //Both reference entity id and edit entity id both are same for primary tab
          compare.isEqual(refEntityId, '') ? <AxWizard  {...{
            refEntityId: refEntityId, editEntityId: refEntityId, tabJson: entityData.entityJSON.tabs[0],
            module: entityData.module, entityName: entityData.entityName,
            displayName: entityData.entityJSON.displayName,
            icon: entityData.entityJSON.icon, ...props
          }} handleFromSaved={handleFromSaved} />
            :
            // when primary entity id has value then redirect to tabs screen
            <ReactCSSTransitionGroup
              component="div"
              transitionName="TabsAnimation"
              transitionAppear={true}
              transitionAppearTimeout={0}
              transitionEnter={false}
              transitionLeave={false}>
              <div className="app-page-title pt-2 pb-2">
                <div className="page-title-wrapper">
                  <div className="page-title-heading">
                    <div>{entityData.entityJSON.displayName}Ax</div>
                  </div>
                </div>
              </div>
              <Tabs
                defaultActiveKey="0"
                renderTabBar={() => <ScrollableInkTabBar onTabClick={this.onTabClick} />}
                // renderTabBar={() => <ScrollableInkTabBar />}
                renderTabContent={() => <TabContent />}
              >
                {
                  entityData.entityJSON.tabs.map((tab, key) => {
                    return (() => {
                      return compare.isEqual(key, 0) ?
                        //Both reference entity id and edit entity id both are same for primary tab
                        <TabPane tab={tab.displayName} key={key} placeholder="loading...">
                          <AxWizard  {...{
                            refEntityId: refEntityId, editEntityId: refEntityId,
                            tabJson: tab, module: entityData.module, ...props
                          }} handleFromSaved={handleFromSaved} />
                        </TabPane>

                        :
                        <TabPane tab={tab.displayName} key={key} placeholder="loading...">
                          <AxListSecondary  {...{
                            refEntityId: refEntityId, tabJson: tab,
                            module: entityData.module, ...props
                          }} />
                        </TabPane>
                    })()
                  })}
              </Tabs>
            </ReactCSSTransitionGroup>

        )}
    </Fragment>
  )
}