import React from "react";
import Link from "next/link";

const menuItems = [
  {
    href: "/",
    title: "Homepage",
  },
  {
    href: "/about",
    title: "About",
  },
  {
    href: "/contact",
    title: "Contact",
  },
];

const SidebarDashboard = () => {
  return (
    <aside className="bg-fuchsia-100 w-full md:w-60">
      <nav>
        <ul>
          {menuItems.map(({ href, title }) => (
            <li className="m-2" key={title}>
              {/* <Link href={href}>
                        <a
                          className={`flex p-2 bg-fuchsia-200 rounded hover:bg-fuchsia-400 cursor-pointer ${
                            router.asPath === href &&
                            "bg-fuchsia-600 text-white"
                          }`}
                        >
                          {title}
                        </a>
                      </Link> */}
              <Link
                href={href}
                className={
                  "flex p-2 bg-fuchsia-200 rounded hover:bg-fuchsia-400 cursor-pointer"
                }
              >
                {title}
              </Link>
            </li>
          ))}
        </ul>
      </nav>
    </aside>
  );
};

export default SidebarDashboard;
