import { UserService } from 'src/app/core/services/user.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Output() public drawerToggle = new EventEmitter();

  constructor(private userService: UserService) {}

  ngOnInit() {}

  public onToggleDrawer = () => {
    this.drawerToggle.emit();
  };

  public logout(): void {
    this.userService.logout(true);
  }
}
