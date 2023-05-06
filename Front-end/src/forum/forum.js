import { Tabs, Typography } from '@arco-design/web-react';
import { IconUpload, IconBook, IconUser } from '@arco-design/web-react/icon';
import Blog from './Blog';
import Upload from './own_blog'
import MyBlog from './myblog'
import './forum.css'
import { AuthContext } from "../component/AuthContext";

const TabPane = Tabs.TabPane;
const style = {
  textAlign: 'center',
  marginTop: 0,
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
      <TabPane
        key='3'
        title={
          <span>
            <IconUser style={{ marginRight: 6 }} />
            My Blog
          </span>
        }
      >
        <Typography.Paragraph style={style}><MyBlog/></Typography.Paragraph>
      </TabPane>
    </Tabs>
  );
};

export default App;