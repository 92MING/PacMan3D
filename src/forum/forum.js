import { Tabs, Typography } from '@arco-design/web-react';
import { IconCalendar, IconClockCircle, IconUser, IconUpload, IconBook } from '@arco-design/web-react/icon';
import Blog from './Blog';
import Upload from './own_blog'
import './forum.css'
const TabPane = Tabs.TabPane;
const style = {
  textAlign: 'center',
  marginTop: 20,
};

const App = () => {
  return (
    <Tabs defaultActiveTab='1'>
      <TabPane
        key='1'
        title={
          <span>
            <IconBook style={{ marginRight: 6 }} />
            Blog Square
          </span>
        }
      >
        <Typography.Paragraph style={style}><Blog/></Typography.Paragraph>
      </TabPane>
      <TabPane
        key='2'
        title={
          <span>
            <IconUpload  style={{ marginRight: 6 }} />
            Upload Blog
          </span>
        }
      >
        <Typography.Paragraph style={style}><Upload/></Typography.Paragraph>
      </TabPane>
    </Tabs>
  );
};

export default App;