import '@arco-design/web-react/dist/css/arco.css';
import '../HomePage/HomePage.css';
import PopupMenu from '../PopUpMenu/PopupMenu';
import title_icon from '../../image/pacman-icon.png';
import { Helmet } from 'react-helmet';
import pacman1 from '../../image/pacman-icon.png';
import Forum from '../../forum/forum';
import React ,{useState, useContext}from 'react';
import { Layout, Menu, Breadcrumb, Button, Message } from '@arco-design/web-react';
import { IconCaretRight, IconCaretLeft, IconPalette, IconTrophy, IconStorage, IconUserGroup, IconThunderbolt, IconUser} from '@arco-design/web-react/icon';
import { AuthContext,useAuth } from '../AuthContext';
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { FaUserCircle } from "react-icons/fa";
import {BiLogOut} from "react-icons/bi";

const MenuItem = Menu.Item;
const Sider = Layout.Sider;
const Header = Layout.Header;
const Footer = Layout.Footer;
const Content = Layout.Content;

export default function OnlineForum() {
    const [collapsed, setCollapsed] = useState(false);
    const { user} = useContext(AuthContext);
    const { handleLogout } = useAuth();
    const navigate = useNavigate();


    const handleCollapsed = () => {
    setCollapsed(!collapsed);
    };

    const handleLogoutClick = () => {
        handleLogout();
        navigate("/sign-in");
        toast.success('logged out successfully!', {
            position: "top-center",
            autoClose: 500,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: "light",
            });
      };
    
    
    return (
      <Layout className='layout-collapse-demo'>
        <Sider
          collapsed={collapsed}
          onCollapse={handleCollapsed}
          collapsible
          trigger={collapsed ? <IconCaretRight/> : <IconCaretLeft />}
          breakpoint='xl'
          className='menu'
        >
          <div className='user-info'>
          <FaUserCircle fontSize={45} color='#cc0000'/>
            <h5 className = 'user-text' >{user}</h5>
              {collapsed ?  <button className='logout-btn' onClick= {handleLogoutClick} > 
                                  <BiLogOut fontSize={20}/>
                            </button>: 
               <button className='logout-btn-normal' onClick= {handleLogoutClick} > 
                  <BiLogOut fontSize={28} style={{ marginRight: '2px' }}/> 
                  Logout
               </button>
               }
          </div>

          <Menu
            defaultOpenKeys={['1']}
            defaultSelectedKeys={['0_5']}
            style={{ width: '100%'}}
            className='menu'>
            <div className='div-design'>
            <MenuItem key='0_1'style={{backgroundColor:"rgb(238, 219, 98)"}}>
              <IconTrophy />
              <Button type='text' className='t-btn' shape='round' href='/home-page/'>              
              Start Game
              </Button>
            </MenuItem>
            </div>
            <div className='div-design'>
            <MenuItem key='0_2'style={{backgroundColor:"rgb(238, 219, 98)"}}>
              <IconPalette />
              <Button type='text' className='t-btn' shape='round' href='/home-page/create-map'>
                    Create Map
              </Button>
            </MenuItem>
            </div>
            <div className='div-design'>
            <MenuItem key='0_3' style={{backgroundColor:"rgb(238, 219, 98)"}}>
              <IconStorage />
              <Button type='text' className='t-btn' shape='round' href='/home-page/map-menu'>
                Map Menu
              </Button>
            </MenuItem>
            </div>
           
            <div className='div-design'>
            <MenuItem key='0_4' style={{backgroundColor:"rgb(238, 219, 98)"}}> 
              <IconUserGroup />
              <Button type='text' className='t-btn' shape='round' href='/home-page/online-forum'>
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
            <Content style={{ margin: '16px 0' }}>
          <Forum/>
            </Content>
            <Footer>CSCI3100 ©2023 Created by Group F4</Footer>
          </Layout>
        </Layout>
      </Layout>
    );
  };

