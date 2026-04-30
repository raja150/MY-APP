import React, { Fragment } from 'react'
import Tabs, { TabPane } from 'rc-tabs';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import TabContent from 'rc-tabs/lib/TabContent';
import Attendance from '.';
import * as compare from 'utils/Compare'
import PageHeader from 'Layout/AppMain/PageHeader';
import DownloadAttendance from './Download';

export default function AttendanceReport(props) {
    const attendanceTabs = [
        { id: 1, text: 'Report' },
        { id: 2, text: 'Download' },
    ]
    return <Fragment>
        <PageHeader title={'Attendance '} />

        <Tabs defaultActiveKey="0"
            renderTabBar={() => <ScrollableInkTabBar />}
            renderTabContent={() => <TabContent />}
        >
            {
                attendanceTabs.length > 0 && attendanceTabs.map((tab, key) => {
                    return (
                        <TabPane tab={tab.text} key={key}>
                            {
                                compare.isEqual(key, 0) && <Attendance props={props} />
                            }
                            {
                                compare.isEqual(key, 1) && <DownloadAttendance props={props} />
                            }
                        </TabPane>
                    )
                })
            }

        </Tabs>
    </Fragment>
}