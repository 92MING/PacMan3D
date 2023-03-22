import LoginPage from './LoginPage';
import SignUp from './SignUp';
import PopupMenu from './PopupMenu';
import HomePage from './HomePage';
import ChangePassword from './ChangePassword';

const routes = [
  {
    path: '/',
    component: LoginPage,
    exact: true,
    breadcrumbName: 'LoginPage'
  },
  {
    path: '/sign-up',
    component: SignUp,
    breadcrumbName: 'SignUp'
  },
  {
    path: '/home-page',
    component: HomePage,
    breadcrumbName: 'HomePage',
    routes: [
      {
        path: '/popup-menu',
        component: PopupMenu,
        breadcrumbName: 'PopupMenu'
      },
      {
        path: '/change-password',
        component: ChangePassword,
        breadcrumbName: 'ChangePassword'
      }
    ]
  }
];

export default routes;

/*not finished yet
    change in index.html 32-34 
    <script>
      var mountNode = document.getElementById('root');
    </script>
*/