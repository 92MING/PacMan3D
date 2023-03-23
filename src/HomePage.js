import '@arco-design/web-react/dist/css/arco.css';
import './HomePage.css';
import PopupMenu from './PopupMenu';
import title_icon from './image/pacman-icon.png';
import { Helmet } from 'react-helmet';
import pacman1 from './image/pacman.png';


import React from 'react';
import { Layout, Menu, Breadcrumb, Button, Message } from '@arco-design/web-react';
import { IconCaretRight, IconCaretLeft, IconPalette, IconTrophy, IconStorage, IconUserGroup, IconThunderbolt} from '@arco-design/web-react/icon';

const MenuItem = Menu.Item;
const Sider = Layout.Sider;
const Header = Layout.Header;
const Footer = Layout.Footer;
const Content = Layout.Content;

class App extends React.Component {
  state = {
    collapsed: false,
  };
  handleCollapsed = () => {
    this.setState({
      collapsed: !this.state.collapsed,
    });
  };

  render() {
    return (
      <Layout className='layout-collapse-demo'>
        <Sider
          collapsed={this.state.collapsed}
          onCollapse={this.handleCollapsed}
          collapsible
          trigger={this.state.collapsed ? <IconCaretRight/> : <IconCaretLeft />}
          breakpoint='xl'
          className='menu'
        >
          <div className='logo'>
                <img src={pacman1} width = "40px" ></img>
            </div>
          <Menu
            defaultOpenKeys={['1']}
            defaultSelectedKeys={['0_5']}
            style={{ width: '100%'}}
            className='menu'
          >
            <div className='div-design'>
            <MenuItem key='0_1'style={{backgroundColor:"rgb(238, 219, 98)"}}>
              <IconTrophy />
              <Button type='text' className='t-btn' shape='round'>
              Start Game
              </Button>
            </MenuItem>
            </div>
            <div className='div-design'>
            <MenuItem key='0_2'style={{backgroundColor:"rgb(238, 219, 98)"}}>
              <IconPalette />
              <Button type='text' className='t-btn' shape='round'>
              Create Map
              </Button>
            </MenuItem>
            </div>
            <div className='div-design'>
            <MenuItem key='0_3' style={{backgroundColor:"rgb(238, 219, 98)"}}>
              <IconStorage />
              <Button type='text' className='t-btn' shape='round'>
              Map Menu
              </Button>
            </MenuItem>
            </div>
           
           <div  className='div-design'>
            <MenuItem key='0_4' style={{backgroundColor:"rgb(238, 219, 98)"}}> 
              <IconUserGroup />
              <Button type='text' className='t-btn' shape='round'>
              Online Forum
              </Button>
            </MenuItem>
            </div>
            
            <div className='div-design'>
            <MenuItem key='0_5' style={{backgroundColor:"rgb(238,219,98)"}}>
              <IconThunderbolt/>
              <PopupMenu/>
            </MenuItem>
            </div>
          </Menu>
        </Sider>
        <Layout>
        <Helmet>
            <title>Pac-Man Home Page</title>
        </Helmet>
          <Header style={{ paddingLeft: 20 }}>
          <div className='img'>
                <img src={title_icon} height = "64px" ></img>
            </div>
          </Header>
          <Layout style={{ padding: '0 24px' }}>
            <Content style={{ margin: '16px 0' }}>Content</Content>
            <Footer>CSCI3100 ©2023 Created by Group F4</Footer>
          </Layout>
        </Layout>
      </Layout>
    );
  }
}

export default App;