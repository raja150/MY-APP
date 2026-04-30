import Tabs, { TabPane } from 'rc-tabs';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import TabContent from 'rc-tabs/lib/TabContent';
import React, { Fragment, useState } from "react";
import { Card, CardBody } from "reactstrap";
import * as compare from 'utils/Compare';
import Annual from "./Annual";
import PaySlips from "./payslip";
const Payslip = (props) => {
  const paySlipTabs = [{ id: 0, text: 'Payslips' },{ id: 1, text: 'Annual Earnings' },]
  return (
    <Fragment>
        <Card>
          <CardBody>
              <Fragment>
                <Tabs defaultActiveKey='0'
                  renderTabBar={() => <ScrollableInkTabBar />}
                  renderTabContent={() => <TabContent />}
                >
                  {
                    paySlipTabs.length > 0 && paySlipTabs.map((tab, key) => {
                      return (
                        <TabPane tab={tab.text} key={key}>
                          {
                            compare.isEqual(key, 0) && <PaySlips {...props} />
                          }
                          {
                            compare.isEqual(key, 1) && <Annual {...props} />
                          }
                        </TabPane>
                      )
                    })
                  }
                </Tabs>
              </Fragment>
          </CardBody>
        </Card>
      
    </Fragment>
  );
};
export default Payslip;
