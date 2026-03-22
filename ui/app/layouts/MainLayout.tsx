import Logo from "~/components/Logo/Logo";

export default function MainLayout({ children }: { children: React.ReactNode })
{
    return (
        <div className="min-h-screen flex flex-col">
            <header style={{display:"flex", alignItems:"center", width:"100vw"}}>
                <Logo/>
            </header>

            <main className="flex-1">
            {children}
            </main>

            <footer className="p-5 text-center">
            Trevisharp© 2026
            </footer>
        </div>
    )
}